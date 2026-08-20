using Phoenixx99JD_Zeuspet.Web.Models;
using Phoenixx99JD_Zeuspet.Web.Services;

namespace Phoenixx99JD_Zeuspet.Web.Tests;

public class VeterinariaServiceTests
{
    [Fact]
    public void AgregarActividad_DeberiaCrearActividadParaLaMascota()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var fecha = new DateTime(2026, 8, 20);

        var actividad = service.AgregarActividad(mascota.Id, "Paseo", "08:00 AM", "Solo", fecha);

        Assert.NotNull(actividad.Id);
        Assert.Equal(mascota.Id, actividad.MascotaId);
        Assert.Equal("Paseo", actividad.Nombre);
        Assert.Equal(fecha, actividad.Fecha);
        Assert.Contains(actividad, service.ListarActividadesDeMascota(mascota.Id));
    }

    [Fact]
    public void AgregarCita_DeberiaCrearCitaPendiente()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var cita = service.AgregarCita(mascota.Id, "Control anual", "09:30 AM", "veterinaria");

        Assert.NotNull(cita.Id);
        Assert.Equal(mascota.Id, cita.MascotaId);
        Assert.Equal("Control anual", cita.Titulo);
        Assert.False(cita.Completada);
        Assert.Contains(cita, service.ListarCitasDeMascota(mascota.Id));
    }

    [Fact]
    public void MarcarCitaCompletada_DeberiaActualizarElEstado()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var cita = service.AgregarCita(mascota.Id, "Vacunacion", "11:00 AM", "veterinaria");

        var resultado = service.MarcarCitaCompletada(cita.Id);

        Assert.True(resultado);
        Assert.True(cita.Completada);
        Assert.DoesNotContain(cita, service.ListarProximasCitas());
    }

    [Fact]
    public void EliminarCita_DeberiaQuitarLaCita()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var cita = service.AgregarCita(mascota.Id, "Desparasitacion", "01:00 PM", "veterinaria");

        var resultado = service.EliminarCita(cita.Id);

        Assert.True(resultado);
        Assert.DoesNotContain(cita, service.ListarCitasDeMascota(mascota.Id));
        Assert.False(service.EliminarCita(cita.Id));
    }

    [Fact]
    public void AgregarRegistroPeso_DeberiaGuardarPesoYFecha()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var fecha = new DateTime(2026, 8, 19);

        var registro = service.AgregarRegistroPeso(mascota.Id, 25.3, fecha);

        Assert.NotNull(registro.Id);
        Assert.Equal(mascota.Id, registro.MascotaId);
        Assert.Equal(25.3, registro.Peso);
        Assert.Equal(fecha, registro.Fecha);
        Assert.Contains(registro, service.ListarRegistroPesoDeMascota(mascota.Id));
    }

    [Fact]
    public void AgregarEvaluacion_DeberiaGuardarPuntajesYPromedio()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var evaluacion = service.AgregarEvaluacion(mascota.Id, 8, 9, 7, 10);

        Assert.NotNull(evaluacion.Id);
        Assert.Equal(mascota.Id, evaluacion.MascotaId);
        Assert.Equal(8, evaluacion.Comportamiento);
        Assert.Equal(9, evaluacion.Higiene);
        Assert.Equal(7, evaluacion.Movimiento);
        Assert.Equal(10, evaluacion.Animo);
        Assert.Equal(8.5, evaluacion.Promedio);
        Assert.Contains(evaluacion, service.ListarEvaluacionesDeMascota(mascota.Id));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void AgregarEvaluacion_DeberiaRechazarPuntajeFueraDeRango(int puntaje)
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var excepcion = Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.AgregarEvaluacion(mascota.Id, puntaje, 5, 5, 5));

        Assert.Equal("comportamiento", excepcion.ParamName);
    }

    [Fact]
    public void AgregarConsulta_DeberiaGuardarMotivoYNotas()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var consulta = service.AgregarConsulta(mascota.Id, "Perdida de apetito", "Observar durante 48 horas.");

        Assert.NotNull(consulta.Id);
        Assert.Equal(mascota.Id, consulta.MascotaId);
        Assert.Equal("Perdida de apetito", consulta.Motivo);
        Assert.Equal("Observar durante 48 horas.", consulta.Notas);
        Assert.Contains(consulta, service.ListarConsultasDeMascota(mascota.Id));
    }

    [Fact]
    public void ActualizarDatosMascota_DeberiaActualizarLosDatos()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var resultado = service.ActualizarDatosMascota(
            mascota.Id,
            26.1,
            "Macho",
            "En observacion",
            "Requiere control de peso.",
            "https://example.com/zeus.jpg");

        Assert.True(resultado);
        Assert.Equal(26.1, mascota.Peso);
        Assert.Equal("Macho", mascota.Sexo);
        Assert.Equal("En observacion", mascota.Estado);
        Assert.Equal("Requiere control de peso.", mascota.Notas);
        Assert.Equal("https://example.com/zeus.jpg", mascota.FotoUrl);
    }

    private static VeterinariaService CrearServicio() => new();

    private static Mascota ObtenerZeus(VeterinariaService service) =>
        Assert.Single(service.ListarMascotas(), mascota => mascota.Nombre == "Zeus");
}
