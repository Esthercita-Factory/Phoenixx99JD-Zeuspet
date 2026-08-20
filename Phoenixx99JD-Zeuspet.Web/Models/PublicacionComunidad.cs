namespace Phoenixx99JD_Zeuspet.Web.Models;

public class PublicacionComunidad
{
    public string Id { get; set; }
    public string ClienteId { get; set; }
    public string? MascotaId { get; set; }
    public string Contenido { get; set; }
    public string Categoria { get; set; }
    public DateTime Fecha { get; set; }
    public bool EsVeterinario { get; set; }
    public HashSet<string> LikesDe { get; set; } = [];

    public PublicacionComunidad(
        string id,
        string clienteId,
        string? mascotaId,
        string contenido,
        string categoria,
        DateTime fecha,
        bool esVeterinario)
    {
        Id = id;
        ClienteId = clienteId;
        MascotaId = mascotaId;
        Contenido = contenido;
        Categoria = categoria;
        Fecha = fecha;
        EsVeterinario = esVeterinario;
    }
}
