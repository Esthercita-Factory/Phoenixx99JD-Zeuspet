namespace Phoenixx99JD_Zeuspet.Models;

public class Mascota : Animal, IRegistrable
{
    private string _raza = "";

    // Id solo se asigna en el constructor (get privado = protegido de modificaciones).
    public string Id { get; private set; }
    public string Raza
    {
        get => _raza;
        set => _raza = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
    public string ClienteId { get; }

    public Mascota(string nombre, string especie, string raza, int edad, string clienteId)
        : base(nombre, edad, especie)
    {
        Id = Services.GeneradorId.Generar();
        Raza = raza;
        ClienteId = clienteId;
    }

    // Polimorfismo: el sonido depende de la especie de la mascota.
    public override string EmitirSonido()
    {
        return Especie.ToLowerInvariant() switch
        {
            "perro" => "Guau",
            "gato" => "Miau",
            "conejo" => "...",
            _ => "..."
        };
    }

    public override void MostrarInformacion()
    {
        base.MostrarInformacion();
        System.Console.WriteLine($"  Raza: {Raza}");
        System.Console.WriteLine($"  ID: {Id}");
    }

    public string Registrar()
    {
        return $"Mascota {Nombre} ({Especie}) registrada con ID {Id}.";
    }
}
