namespace Scsl.Unlocode.Core.Query;

public sealed class MdbQueryRequest
{
    public string MdbPath{get; init;} = string.Empty;
    public string? Sql {get; init;}
    public string? Table {get; init;}
    public string? Select {get; init;}
    public string? Where {get; init;}
    public int? Top {get; init;}
}