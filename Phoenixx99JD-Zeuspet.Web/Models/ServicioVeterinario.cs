namespace Phoenixx99JD_Zeuspet.Web.Models;

public abstract class ServicioVeterinario
{
    public string Tipo { get; }
    public string Descripcion { get; }

    protected ServicioVeterinario(string tipo, string descripcion)
    {
        Tipo = tipo;
        Descripcion = descripcion;
    }

    public abstract string Atender(string nombreMascota);
}

public class ConsultaGeneral : ServicioVeterinario
{
    public ConsultaGeneral() : base("ConsultaGeneral", "Revision de signos vitales, peso y diagnostico.") { }

    public override string Atender(string nombreMascota)
    {
        return $"{nombreMascota} paso una consulta general: revision de signos vitales y diagnostico.";
    }
}

public class Vacunacion : ServicioVeterinario
{
    public Vacunacion() : base("Vacunacion", "Aplicacion de vacuna y registro en el esquema de vacunacion.") { }

    public override string Atender(string nombreMascota)
    {
        return $"{nombreMascota} fue vacunado: aplicacion de vacuna y registro en el esquema.";
    }
}
