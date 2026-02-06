using System.Diagnostics.CodeAnalysis;

using Microsoft.Win32;

namespace Scsl.Unlocode.Infrastructure.Mdb;

public static class AccessProviderAvailability
{
    public static bool IsAceInstalled() => GetBestAceProvider() != null;

    public static string? GetBestAceProvider()
    {
        if(IsProgIdRegistered("Microsoft.ACE.OLEDB.16.0"))
            return "Microsoft.ACE.OLEDB.16.0";

        if(IsProgIdRegistered("Microsoft.ACE.OLEDB.15.0"))
            return "Microsoft.ACE.OLEDB.15.0";

        if (IsProgIdRegistered("Microsoft.ACE.OLEDB.12.0"))
            return "Microsoft.ACE.OLEDB.12.0";

        return null;
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private static bool IsProgIdRegistered(string progId)
    {
        return Registry.ClassesRoot.OpenSubKey(progId) != null;
    }
}