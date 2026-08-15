namespace Phoenixx99JD_Zeuspet.Models;

// Clase abstracta: define el contrato Atender() que cada servicio debe implementar.
public abstract class ServicioVeterinario
{
    public abstract string Atender();
}

public class ConsultaGeneral : ServicioVeterinario
{
    public override string Atender()
    {
        return "Consulta general: revision de signos vitales, peso y diagnostico.";
    }
}

public class Vacunacion : ServicioVeterinario
{
    public override string Atender()
    {
        return "Vacunacion: aplicacion de vacuna y registro en el esquema de vacunacion.";
    }
}
