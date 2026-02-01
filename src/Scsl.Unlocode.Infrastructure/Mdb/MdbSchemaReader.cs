using System.Data;
using System.Data.OleDb;

using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public class MdbSchemaReader : IMdbSchemaReader
{
    public IReadOnlyList<MdbTableInfo> GetTables(string mdbPath)
    {
        var connString = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={mdbPath};Persist Security Info=False;";

        using var conn = new OleDbConnection(connString);
        conn.Open();

        DataTable schema = conn.GetSchema("Tables");

        var result = new List<MdbTableInfo>();

        foreach (DataRow row in schema.Rows)
        {
            string name = row["TABLE_NAME"].ToString();
            string type = row["TABLE_TYPE"].ToString();

            if (type == "TABLE" && !name.StartsWith("MSys"))
            {
                result.Add(new MdbTableInfo()
                {
                    Name = name,
                    Type = type
                });
            }
        }


        return result;
    }
}