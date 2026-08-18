namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Actividad
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public string Nombre { get; set; }
    public string Hora { get; set; }
    public string Grupo { get; set; }
    public DateTime Fecha { get; set; }

    public Actividad(string id, string mascotaId, string nombre, string hora, string grupo, DateTime fecha)
    {
        Id = id;
        MascotaId = mascotaId;
        Nombre = nombre;
        Hora = hora;
        Grupo = grupo;
        Fecha = fecha;
    }
}
