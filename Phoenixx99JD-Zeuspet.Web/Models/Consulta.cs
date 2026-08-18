namespace Phoenixx99JD_Zeuspet.Web.Models;

public class Consulta
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public DateTime Fecha { get; set; }
    public string Motivo { get; set; }
    public string Notas { get; set; }

    public Consulta(string id, string mascotaId, DateTime fecha, string motivo, string notas)
    {
        Id = id;
        MascotaId = mascotaId;
        Fecha = fecha;
        Motivo = motivo;
        Notas = notas;
    }
}
