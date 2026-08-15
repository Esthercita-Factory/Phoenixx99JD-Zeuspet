namespace Phoenixx99JD_Zeuspet.Models;

// Clase base abstracta: no se puede instanciar, solo sirve como plantilla
// para los animales reales (por ahora, Mascota).
public abstract class Animal
{
    private string _nombre = "";
    private int _edad;
    private string _especie = "";

    // Encapsulacion: campos privados expuestos mediante propiedades que validan.
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

    public string Especie
    {
        get => _especie;
        set => _especie = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }

    protected Animal(string nombre, int edad, string especie)
    {
        Nombre = nombre;
        Edad = edad;
        Especie = especie;
    }

    // Metodo virtual: puede ser sobrescrito por las clases derivadas.
    public virtual string EmitirSonido() => "...";

    public virtual void MostrarInformacion()
    {
        System.Console.WriteLine($"  Nombre: {Nombre}");
        System.Console.WriteLine($"  Especie: {Especie}");
        System.Console.WriteLine($"  Edad: {Edad} anios");
    }
}
