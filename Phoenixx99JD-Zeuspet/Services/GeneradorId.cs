namespace Phoenixx99JD_Zeuspet.Services;

public static class GeneradorId
{
    public static string Generar() => Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
}
