using System.Text.Json;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Unlocode.DataImporter.Configuration;

public static class ExternalDatabaseConfigLoader
{
    public static ExternalDatabaseConfig Load(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();

        return ext switch
        {
            ".json" => JsonSerializer.Deserialize<ExternalDatabaseConfig>(
                File.ReadAllText(filePath),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!,

            ".yaml" or ".yml" => new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build().Deserialize<ExternalDatabaseConfig>(File.ReadAllText(filePath))!,

            _ => throw new InvalidOperationException("Config file must be JSON or YAML format.")
        };
    }
}