namespace Phoenixx99JD_Zeuspet.Web.Models;

public class RegistroPeso
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public double Peso { get; set; }
    public DateTime Fecha { get; set; }

    public RegistroPeso(string id, string mascotaId, double peso, DateTime fecha)
    {
        Id = id;
        MascotaId = mascotaId;
        Peso = peso;
        Fecha = fecha;
    }
}
