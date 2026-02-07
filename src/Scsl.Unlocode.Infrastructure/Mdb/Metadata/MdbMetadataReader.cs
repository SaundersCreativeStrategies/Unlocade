using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Metadata;
using Scsl.Unlocode.Infrastructure.Mdb.Factory;
using Scsl.Unlocode.Infrastructure.Mdb.Mapper;

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
        try
        {
            using var connection = OleDbConnectionFactory.Create(mdbPath, _diagnostics);

            using (new DiagnosticsScope(
                       _diagnostics!,
                       DiagnosticsEvents.MdbListTablesStart,
                       DiagnosticsEvents.MdbListTablesCompleted,
                       $"Opening MDB connection"))
            {
                connection.Open();
            }

            DataTable tables;
            using (new DiagnosticsScope(
                       _diagnostics!,
                       DiagnosticsEvents.MdbReadSchemaStart,
                       DiagnosticsEvents.MdbReadSchemaCompleted,
                       $"Reading Tables schema"))
            {
                tables = connection.GetSchema("Tables");
            }

            var results = new List<MdbTableInfo>();

            using (new DiagnosticsScope(
                       _diagnostics!,
                       DiagnosticsEvents.MdbListTablesStart,
                       DiagnosticsEvents.MdbListTablesCompleted,
                       $"Building tables list"))
            {
                foreach (DataRow row in tables.Rows)
                {
                    string? name = row["TABLE_NAME"].ToString();
                    string? type = row["TABLE_TYPE"].ToString();

                    if (type == "TABLE" && name is not null && !name.StartsWith("MSys"))
                    {
                        results.Add(new MdbTableInfo() { Name = name, Type = type });
                    }
                }

                _diagnostics?.LogInfo(DiagnosticsEvents.MdbTablesFound, $"Tables found: {results.Count}");
                return results;
            }
        }
        catch (Exception ex)
        {
            _diagnostics?.LogError(DiagnosticsEvents.Error, "Failed to read tables from MDB.", ex);
            throw;
        }
    }

    public IReadOnlyList<MdbColumnInfo> GetTableSchema(string mdbPath, string tableName)
    {
        if (string.IsNullOrWhiteSpace(mdbPath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(mdbPath));

        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));

        try
        {
            using var connection = OleDbConnectionFactory.Create(mdbPath, _diagnostics);

            // open MDB
            using (new DiagnosticsScope(
                       _diagnostics!,
                       DiagnosticsEvents.MdbOpen,
                       DiagnosticsEvents.MdbOpenCompleted,
                       "Opening MDB connection"))
            {
                connection.Open();
            }

            // read schema
            List<MdbColumnInfo> columns;

            using (new DiagnosticsScope(
                       _diagnostics!,
                       DiagnosticsEvents.MdbReadSchemaStart,
                       DiagnosticsEvents.MdbReadSchemaCompleted,
                       $"Reading schema for table '{tableName}'"))
            {
                var columnSchema = connection.GetSchema("Columns", [null, null, tableName, null]);

                columns = columnSchema.Rows
                    .Cast<DataRow>()
                    .Select(r => new MdbColumnInfo
                    {
                        Name = r["COLUMN_NAME"].ToString()!,
                        DataType = OleDbTypeMapper.ToFriendyName(r["DATA_TYPE"].ToString()!),
                        OleDbType = OleDbTypeMapper.ToOleDbType(r["DATA_TYPE"]),
                        Size = r["CHARACTER_MAXIMUM_LENGTH"] as int?,
                        IsNullable = r["IS_NULLABLE"].ToString() == "YES"
                    }).ToList();
            }

            var columnMap = columns.ToDictionary(
                c => c.Name,
                StringComparer.InvariantCultureIgnoreCase);

            // Index / PK / Unique detection
            if (HasSchema(connection, "Indexs"))
            {
                using (new DiagnosticsScope(
                           _diagnostics!,
                           DiagnosticsEvents.MdbReadIndexesStart,
                           DiagnosticsEvents.MdbReadIndexesCompleted,
                           $"Reading index metadata for table '{tableName}'"))
                {
                    var indexSchema = connection.GetSchema("IndexColumns", [null, null, null, tableName]);

                    foreach (DataRow row in indexSchema.Rows)
                    {
                        var columnName = row["COLUMN_NAME"]?.ToString();
                        if (columnName == null || !columnMap.TryGetValue(columnName, out var column))
                            continue;

                        column.IsIndexed = true;

                        if (row["PRIMARY_KEY"] is bool and true)
                            column.IsPrimaryKey = true;

                        if (row["UNIQUE"] is bool and true)
                            column.IsUnique = true;
                    }
                }
            }
            else
            {
                _diagnostics!.LogInfo(
                    DiagnosticsEvents.MdbReadIndexesCompleted,
                    "Index metadata not available in this MDB");
            }

            // Foreign key detection (Access relationships)
            try
            {
                using (new DiagnosticsScope(
                           _diagnostics!,
                           DiagnosticsEvents.MdbReadForeignKeysStart,
                           DiagnosticsEvents.MdbReadForeignKeysCompleted,
                           $"Reading foreign keys for table '{tableName}'"
                       ))
                {
                    var fkSchema = connection.GetSchema("Foreign Keys", [null, null, tableName]);

                    foreach (DataRow row in fkSchema.Rows)
                    {
                        var fkColumn = row["FK_COLUMN_NAME"]?.ToString();
                        if (fkColumn == null || !columnMap.TryGetValue(fkColumn, out var column))
                            continue;
                        column.IsForeignKey = true;
                        column.ReferencesTable = row["PK_TABLE_NAME"]?.ToString();
                        column.ReferencesColumn = row["PK_COLUMN_NAME"]?.ToString();
                    }
                }
            }
            catch
            {
                // Access does not always expose FK metadata
                // Intentionally ignored (best-effort)
            }

            _diagnostics!.LogInfo(DiagnosticsEvents.MdbSchemaColumnsFound, $"Columns found: {columns.Count}");
            return columns;
        }
        catch (Exception ex)
        {
            _diagnostics!.LogError(
                DiagnosticsEvents.Error,
                $"Failed to read schema for table '{tableName}'",
                ex);
            throw;
        }
    }

    private static bool HasSchema(OleDbConnection connection, string name)
    {
        var schemas = connection.GetSchema("MetaDataCollections");
        return schemas?.Rows
            .Cast<DataRow>()
            .Any(r => r["CollectionName"]?.ToString() == name) == true;
    }
}