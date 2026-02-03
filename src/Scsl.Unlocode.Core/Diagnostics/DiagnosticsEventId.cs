namespace Scsl.Unlocode.Core.Diagnostics;

public readonly record struct DiagnosticsEventId(int Id, string Name)
{
    public override string ToString() => $"{Name}({Id})";
}