namespace Phoenixx99JD_Zeuspet.Models;

public class Mascota
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }
    public int Edad { get; set; }
    public string ClienteId { get; set; }

    public Mascota(string nombre, string especie, string raza, int edad, string clienteId)
    {
        Id = Services.GeneradorId.Generar();
        Nombre = nombre.Trim();
        Especie = especie.Trim();
        Raza = raza.Trim();
        Edad = edad;
        ClienteId = clienteId;
    }
}
