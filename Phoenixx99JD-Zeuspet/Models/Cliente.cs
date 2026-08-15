namespace Phoenixx99JD_Zeuspet.Models;

public class Cliente : IRegistrable
{
    // Encapsulacion: campos privados expuestos mediante propiedades que validan.
    private string _nombre = "";
    private int _edad;
    private string _telefono = "";
    private string _email = "";
    private string _direccion = "";

    // Id solo se asigna en el constructor (get privado = protegido de modificaciones).
    public string Id { get; private set; }
    public string Nombre
    {
        get => _nombre;
        set => _nombre = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
    public int Edad
    {
        get => _edad;
        set => _edad = value >= 0 ? value : 0;
    }
    public string Telefono
    {
        get => _telefono;
        set => _telefono = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
    public string Email
    {
        get => _email;
        set => _email = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
    public string Direccion
    {
        get => _direccion;
        set => _direccion = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }

    // Asociacion: un Cliente (dueno) posee una lista de mascotas.
    public List<Mascota> Mascotas { get; } = [];

    public Cliente(string nombre, string telefono, string email, string direccion, int edad = 0)
    {
        Id = Services.GeneradorId.Generar();
        Nombre = nombre;
        Edad = edad;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
    }

    public void MostrarInformacion()
    {
        System.Console.WriteLine($"  ID: {Id}");
        System.Console.WriteLine($"  Nombre: {Nombre}");
        System.Console.WriteLine($"  Edad: {Edad} anios");
        System.Console.WriteLine($"  Telefono: {Telefono}");
        System.Console.WriteLine($"  Email: {Email}");
        System.Console.WriteLine($"  Direccion: {Direccion}");
        System.Console.WriteLine($"  Mascotas: {Mascotas.Count}");
    }

    // Recorre la lista de mascotas para mostrarlas en pantalla.
    public void MostrarMascotas()
    {
        if (Mascotas.Count == 0)
        {
            System.Console.WriteLine("  No tiene mascotas registradas.");
            return;
        }

        foreach (var m in Mascotas)
            System.Console.WriteLine($"  - {m.Nombre} ({m.Especie}, {m.Raza}, {m.Edad} anios)");
    }

    public string Registrar()
    {
        return $"Cliente {Nombre} registrado con ID {Id}.";
    }
}
