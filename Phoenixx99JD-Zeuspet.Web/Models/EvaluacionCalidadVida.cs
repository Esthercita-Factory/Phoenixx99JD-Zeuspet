namespace Phoenixx99JD_Zeuspet.Web.Models;

public class EvaluacionCalidadVida
{
    public string Id { get; set; }
    public string MascotaId { get; set; }
    public int Comportamiento { get; set; }
    public int Higiene { get; set; }
    public int Movimiento { get; set; }
    public int Animo { get; set; }
    public DateTime Fecha { get; set; }

    public double Promedio => (Comportamiento + Higiene + Movimiento + Animo) / 4.0;

    public EvaluacionCalidadVida(
        string id,
        string mascotaId,
        int comportamiento,
        int higiene,
        int movimiento,
        int animo,
        DateTime fecha)
    {
        Id = id;
        MascotaId = mascotaId;
        Comportamiento = comportamiento;
        Higiene = higiene;
        Movimiento = movimiento;
        Animo = animo;
        Fecha = fecha;
    }
}
