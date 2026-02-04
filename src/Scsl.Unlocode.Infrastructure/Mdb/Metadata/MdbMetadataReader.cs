using System.Data;
using System.Diagnostics.CodeAnalysis;

using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Metadata;
using Scsl.Unlocode.Infrastructure.Mdb.Factory;

namespace Scsl.Unlocode.Infrastructure.Mdb.Metadata;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public sealed class MdbMetadataReader : IMdbMetadataReader
{
    private readonly IDiagnosticsSink? _diagnostics;

    public MdbMetadataReader(IDiagnosticsSink? diagnostics = null)
    {
        _diagnostics = diagnostics ?? NullDiagnosticsSink.Instance;
    }

    public IReadOnlyList<MdbTableInfo> GetTables(string mdbPath)
    {
        using var scope = new DiagnosticsScope(
            _diagnostics!,
            DiagnosticsEvents.MdbListTablesStart,
            DiagnosticsEvents.MdbListTablesCompleted,
            "Listing MDB tables");

        using var connection = OleDbConnectionFactory.Create(mdbPath, _diagnostics);

        _diagnostics?.LogInfo(DiagnosticsEvents.MdbOpen, $"Opening MDB file: {mdbPath}");
        connection.Open();

        _diagnostics?.LogInfo(DiagnosticsEvents.MdbListTables, "Retrieving table list from MDB");
        DataTable schema = connection.GetSchema("Tables");

        var result = new List<MdbTableInfo>();

        foreach (DataRow row in schema.Rows)
        {
            string? name = row["TABLE_NAME"].ToString();
            string? type = row["TABLE_TYPE"].ToString();

            if (type == "TABLE" && !name.StartsWith("MSys"))
            {
                result.Add(new MdbTableInfo() { Name = name, Type = type });
            }
        }

        _diagnostics?.LogInfo(DiagnosticsEvents.MdbTablesFound, $"Tables found: {result.Count}");
        return result;
    }

    public IReadOnlyList<MdbColumnInfo> GetTableSchema(string mdbPath, string tableName)
    {
        if (string.IsNullOrWhiteSpace(mdbPath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(mdbPath));

        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));

        try
        {
            // open MDB
            using var scope = new DiagnosticsScope(
                _diagnostics!,
                DiagnosticsEvents.MdbOpen,
                DiagnosticsEvents.MdbOpenCompleted,
                "Opening MDB");

            using var conneciton = OleDbConnectionFactory.Create(mdbPath, _diagnostics);
            conneciton.Open();

            // read schema
            using var schemaScope = new DiagnosticsScope(
                _diagnostics!,
                DiagnosticsEvents.MdbReadSchema,
                DiagnosticsEvents.MdbReadSchemaCompleted,
                $"Reading schema for table '{tableName}'");

            // columns
            var columnSchema = conneciton.GetSchema("Columns", [null, null, tableName, null]);

            var columns = columnSchema.Rows.Cast<DataRow>().Select(r => new MdbColumnInfo
            {
                Name = r["COLUMN_NAME"].ToString()!,
                DataType = r["DATA_TYPE"].ToString()!,
                Size = r["CHARACTER_MAXIMUM_LENGTH"] as int?,
                IsNullable = r["IS_NULLABLE"].ToString() == "YES"
            }).ToList();

            var columnMap = columns.ToDictionary(c => c.Name, StringComparer.InvariantCultureIgnoreCase);

            // Index / PK / Unique detection
            var indexSchema = conneciton.GetSchema("IndexColumns", [null, null, null, tableName]);

            foreach (DataRow row in indexSchema.Rows)
            {
                var columnName = row["COLUMN_NMAE"].ToString();
                if (columnName == null || !columnMap.TryGetValue(columnName, out var column))
                    continue;

                column.IsIndexed = true;

                if (row["PRIMARY_KEY"] is bool and true)
                    column.IsPrimaryKey = true;

                if (row["UNIQUE"] is bool and true)
                    column.IsUnique = true;
            }

            // Foreign key detection (Access relationships)
            DataTable fkSchema;
            try
            {
                fkSchema = conneciton.GetSchema("Foreign Keys", [null, null, tableName]);
            }
            catch (Exception e)
            {
                fkSchema = null!;
            }

            if (fkSchema != null)
            {
                foreach (DataRow row in fkSchema.Rows)
                {
                    var fkColumn = row["FK_COLUMN_NAME"].ToString();
                    if (fkColumn == null || !columnMap.TryGetValue(fkColumn, out var column))
                        continue;
                    column.IsForeignKey = true;
                    column.ReferencesTable = row["PK_TABLE_NAME"].ToString();
                    column.ReferencesColumn = row["PK_COLUMN_NAME"].ToString();
                }
            }
            return columns;
        }
        catch (Exception ex)
        {
            _diagnostics!.LogError(
                DiagnosticsEvents.MdbReadSchema,
                $"Failed to read schema for table '{tableName}'",
                ex);
            throw;
        }
    }
}