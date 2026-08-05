namespace Phoenixx99JD_Zeuspet.Services;

public static class GeneradorId
{
    private static readonly Random _random = new();
    private const string Caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Generar()
    {
        return new string(Enumerable.Range(0, 8)
            .Select(_ => Caracteres[_random.Next(Caracteres.Length)])
            .ToArray());
    }
}
