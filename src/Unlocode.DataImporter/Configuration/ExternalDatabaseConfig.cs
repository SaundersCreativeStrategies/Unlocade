namespace Unlocode.DataImporter.Configuration;

public sealed class ExternalDatabaseConfig
{
    public string Provider { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
}