namespace Phoenixx99JD_Zeuspet.Models;

public class Cliente
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public List<Mascota> Mascotas { get; set; } = [];

    public Cliente(string nombre, string telefono, string email, string direccion)
    {
        Id = Services.GeneradorId.Generar();
        Nombre = nombre.Trim();
        Telefono = telefono.Trim();
        Email = email.Trim();
        Direccion = direccion.Trim();
    }
}
