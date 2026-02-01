using System.Data;

using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public class MdbSchemaReader : IMdbSchemaReader
{
    public IReadOnlyList<MdbTableInfo> GetTables(string mdbPath)
    {
        using var conn = OleDbConnectionFactory.Create(mdbPath);
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