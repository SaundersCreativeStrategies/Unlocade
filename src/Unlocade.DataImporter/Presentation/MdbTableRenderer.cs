using Scsl.Unlocode.Core.Metadata;

using Spectre.Console;

namespace Unlocade.DataImporter.Presentation;

public class MdbTableRenderer
{
    public void Render(IReadOnlyList<MdbTableInfo> tables)
    {
        var table = new Table()
            .Title("[yellow]MDB Tables[/]")
            .AddColumn("[cyan]Name[/]")
            .AddColumn("[green]Type[/]");

        foreach (var t in tables)
        {
            table.AddRow(t.Name, t.Type);
        }

        AnsiConsole.Write(table);
    }
}