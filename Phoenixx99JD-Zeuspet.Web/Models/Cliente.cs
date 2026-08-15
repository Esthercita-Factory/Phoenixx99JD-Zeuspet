namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Cliente
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public List<Mascota> Mascotas { get; set; } = [];

    public Cliente(string id, string nombre, int edad, string telefono, string email, string direccion)
    {
        Id = id;
        Nombre = nombre;
        Edad = edad;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
    }
}
