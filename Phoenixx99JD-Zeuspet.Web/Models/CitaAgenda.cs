namespace Phoenixx99JD_Zeuspet.Web.Models;

public class CitaAgenda
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public string Titulo { get; set; }
    public string Hora { get; set; }
    public string Tipo { get; set; }
    public bool Completada { get; set; } = false;

    public CitaAgenda(string id, string mascotaId, string titulo, string hora, string tipo)
    {
        Id = id;
        MascotaId = mascotaId;
        Titulo = titulo;
        Hora = hora;
        Tipo = tipo;
    }
}
