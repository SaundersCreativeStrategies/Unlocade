using System.Data.OleDb;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public static class OleDbConnectionFactory
{
    private const string Provider = "Microsoft.ACE.OLEDB.16.0";

    public static OleDbConnection Create(string filePath)
    {
        if(string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("MDB path cannot be null or empty", nameof(filePath));

        var connectionString = $"Provider={Provider};Data Source={filePath};Persist Security Info=False;";
        return new OleDbConnection(connectionString);
    }
}