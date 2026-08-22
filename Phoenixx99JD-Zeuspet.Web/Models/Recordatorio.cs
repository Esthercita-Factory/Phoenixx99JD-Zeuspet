namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Recordatorio
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public string Titulo { get; set; }
    public DateTime Fecha { get; set; }
    public string Hora { get; set; }

    public Recordatorio(string id, string mascotaId, string titulo, DateTime fecha, string hora)
    {
        Id = id;
        MascotaId = mascotaId;
        Titulo = titulo;
        Fecha = fecha.Date;
        Hora = hora;
    }
}
