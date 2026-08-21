namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Notificacion
{
    public string Id { get; set; }
    public string ClienteId { get; set; }
    public string Mensaje { get; set; }
    public DateTime Fecha { get; set; }
    public bool Leida { get; set; } = false;
    public string Tipo { get; set; }

    public Notificacion(string id, string clienteId, string mensaje, DateTime fecha, string tipo)
    {
        Id = id;
        ClienteId = clienteId;
        Mensaje = mensaje;
        Fecha = fecha;
        Tipo = tipo;
    }
}
