using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

namespace Scsl.Unlocode.Infrastructure.Mdb.Mapper;

internal static class OleDbTypeMapper
{
    public static string ToFriendyName(object? datatype)
    {
        if (datatype is null)
            return "Unknown";

        var code = Convert.ToInt32(datatype);

        return code switch
        {
            130 => "TEXT",          // Unicode text (Access default)
            202 => "VARCHAR",
            203 => "MEMO",
            3   => "INTEGER",
            4   => "SINGLE",
            5   => "DOUBLE",
            7   => "DATETIME",
            11  => "YESNO",
            17  => "BYTE",
            72  => "GUID",
            128 => "BINARY",
            _   => $"UNKNOWN({code})"
        };
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public static OleDbType ToOleDbType(object? dataType)
    {
        if(dataType is null)
            return OleDbType.Empty;

        return (OleDbType)Convert.ToInt32(dataType);
    }
}