namespace Phoenixx99JD_Zeuspet.Web.Models;

public abstract class Animal
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Especie { get; set; }

    protected Animal(string nombre, int edad, string especie)
    {
        Nombre = nombre;
        Edad = edad;
        Especie = especie;
    }

    public virtual string EmitirSonido() => "...";
}
