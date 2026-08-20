namespace Phoenixx99JD_Zeuspet.Web.Models;

public class ComentarioComunidad
{
    public string Id { get; set; }
    public string PublicacionId { get; set; }
    public string ClienteId { get; set; }
    public string Contenido { get; set; }
    public DateTime Fecha { get; set; }

    public ComentarioComunidad(
        string id,
        string publicacionId,
        string clienteId,
        string contenido,
        DateTime fecha)
    {
        Id = id;
        PublicacionId = publicacionId;
        ClienteId = clienteId;
        Contenido = contenido;
        Fecha = fecha;
    }
}
