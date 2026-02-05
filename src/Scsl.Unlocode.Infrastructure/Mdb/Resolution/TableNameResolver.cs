using Scsl.Unlocode.Core.Diagnostics;
using Scsl.Unlocode.Core.Metadata;

namespace Scsl.Unlocode.Infrastructure.Mdb.Resolution;

/// <summary>
/// Resolves a user-provided table name to the canonical MDB table name.
/// Matching is performed in ordered stages to avoid ambiguity.
/// </summary>
public static class TableNameResolver
{
    public static string Resolve(
        string requestName,
        IReadOnlyList<MdbTableInfo> tables,
        IDiagnosticsSink diagnostics)
    {
        if (string.IsNullOrWhiteSpace(requestName))
            throw new ArgumentException("Table name cannot be empty.", nameof(requestName));

        if(tables is null || tables.Count == 0)
            throw new ArgumentException("No tables were loaded from the database.");

        //Exact match (case-sensitive)
        var exact = tables.FirstOrDefault(t =>
            string.Equals(t.Name, requestName, StringComparison.Ordinal));

        if (exact != null) return exact.Name;

        // Exact match (case-insensitive)
        var insensitive = tables.FirstOrDefault(t =>
            string.Equals(
                t.Name, requestName, StringComparison.OrdinalIgnoreCase));

        if (insensitive != null)
        {
            diagnostics.LogInfo(DiagnosticsEvents.MdbTablesFound,
                $"Table name normalized from '{requestName}' to '{insensitive.Name}'.");

            return insensitive.Name;
        }

        // optional: whitespace normalization
        var normalized = requestName.Trim();

        var loose = tables.FirstOrDefault(t =>
            string.Equals(
                t.Name.Trim(), normalized, StringComparison.OrdinalIgnoreCase));

        if (loose != null)
        {
            diagnostics.LogInfo(DiagnosticsEvents.MdbTablesFound,
                $"Table name loosely matched '{requestName} to '{loose.Name}'");

            return loose.Name;
        }

        // Partial / contains match (case-insensitive)
        //     Only allowed if unique
        var partialMatches = tables
            .Where(t =>
                t.Name.Contains(requestName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (partialMatches.Count == 1)
        {
            diagnostics.LogInfo(DiagnosticsEvents.MdbTablesFound,
                $"Table name partially matched '{requestName}' to '{partialMatches[0].Name}'");

            return partialMatches[0].Name;
        }

        if (partialMatches.Count > 1)
        {
            var candidates = string.Join(
                ", ",
                partialMatches.Select(t => $"'{t.Name}'"));

            throw new InvalidOperationException(
                $"table Name '{requestName}' is ambiguous. Matches: {candidates}");
        }

        // No match
        throw new InvalidOperationException(
            $"Table '{requestName}' was not found (case-insensitive match attempted).");
    }
}