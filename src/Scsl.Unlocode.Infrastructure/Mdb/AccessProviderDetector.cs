namespace Scsl.Unlocode.Infrastructure.Mdb;

public static class AccessProviderDetector
{
    public static string SelectProvider(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();

        // ACCDB requires ACE
        if (ext is ".accdb" or ".accde")
        {
            if (!AccessProviderAvailability.IsAceInstalled())
            {
                throw new InvalidOperationException(
                    "This database requires the Microsoft ACE OLE DB provider, " +
                    "but it s not installed.");
            }

            return AccessProviderAvailability.GetBestAceProvider()!;
        }

        // MDB / MDE -> prefer ACE, fallback to Jet
        if(AccessProviderAvailability.IsAceInstalled())
            return AccessProviderAvailability.GetBestAceProvider()!;

        return "Microsoft.Jet.OLEDB.4.0";
    }
}