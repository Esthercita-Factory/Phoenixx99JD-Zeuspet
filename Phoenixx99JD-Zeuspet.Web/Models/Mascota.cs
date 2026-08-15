namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Mascota : Animal
{
    public string Id { get; set; }
    public string Raza { get; set; }
    public string ClienteId { get; set; }

    public Mascota(string id, string nombre, string especie, string raza, int edad, string clienteId)
        : base(nombre, edad, especie)
    {
        Id = id;
        Raza = raza;
        ClienteId = clienteId;
    }

    public override string EmitirSonido()
    {
        return Especie.ToLowerInvariant() switch
        {
            "perro" => "Guau",
            "gato" => "Miau",
            _ => "..."
        };
    }
}
