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
        Assert.Equal("Confirmada", cita.Estado);
        Assert.Contains(cita, service.ListarCitasDeMascota(mascota.Id));
    }

    [Fact]
    public void SolicitarCita_DeberiaCrearUnaCitaPendiente()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var cita = service.SolicitarCita(mascota.Id, "Consulta general", "09:00 AM", "veterinaria");

        Assert.Equal("Pendiente", cita.Estado);
        Assert.Contains(cita, service.ListarCitasPendientes());
    }

    [Fact]
    public void ConfirmarYRechazarCita_DeberianActualizarEstadoYNotificarAlDueno()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var citaConfirmada = service.SolicitarCita(mascota.Id, "Vacunación", "10:00 AM", "veterinaria");
        var citaRechazada = service.SolicitarCita(mascota.Id, "Aseo", "11:00 AM", "aseo");

        Assert.True(service.ConfirmarCita(citaConfirmada.Id));
        Assert.True(service.RechazarCita(citaRechazada.Id));

        Assert.Equal("Confirmada", citaConfirmada.Estado);
        Assert.Equal("Rechazada", citaRechazada.Estado);
        Assert.Empty(service.ListarCitasPendientes());

        var notificaciones = service.ListarNotificacionesDeCliente(mascota.ClienteId);
        Assert.Contains(notificaciones, notificacion => notificacion.Tipo == "Cita" && notificacion.Mensaje.Contains("confirmada"));
        Assert.Contains(notificaciones, notificacion => notificacion.Tipo == "Cita" && notificacion.Mensaje.Contains("rechazada"));
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
    public void SolicitarCita_ConFecha_DeberiaGuardarLaFecha()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var fecha = new DateTime(2026, 8, 25);

        var cita = service.SolicitarCita(mascota.Id, "Consulta general", "09:00 AM", "veterinaria", fecha);

        Assert.Equal(fecha, cita.Fecha);
    }

    [Fact]
    public void ConfirmarCita_DeberiaGenerarActividadPendienteDeLaMascota()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var fecha = new DateTime(2026, 8, 28);
        var cita = service.SolicitarCita(mascota.Id, "Vacunación", "10:00 AM", "veterinaria", fecha);

        Assert.True(service.ConfirmarCita(cita.Id));
        Assert.Equal("Confirmada", cita.Estado);

        var actividades = service.ListarActividadesDeMascota(mascota.Id);
        var actividadCita = actividades.Single(actividad => actividad.CitaId == cita.Id);
        Assert.Equal("Vacunación", actividadCita.Nombre);
        Assert.Equal(fecha, actividadCita.Fecha);
        Assert.Equal("10:00 AM", actividadCita.Hora);
    }

    [Fact]
    public void RechazarCita_NoDeberiaDejarActividadGenerada()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var cita = service.SolicitarCita(mascota.Id, "Aseo", "11:00 AM", "aseo");

        Assert.True(service.RechazarCita(cita.Id));

        Assert.DoesNotContain(service.ListarActividadesDeMascota(mascota.Id), actividad => actividad.CitaId == cita.Id);
    }

    [Fact]
    public void EliminarCitaConfirmada_DeberiaEliminarLaActividadVinculada()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var cita = service.AgregarCita(mascota.Id, "Control", "03:00 PM", "veterinaria");

        Assert.Contains(service.ListarActividadesDeMascota(mascota.Id), actividad => actividad.CitaId == cita.Id);

        Assert.True(service.EliminarCita(cita.Id));
        Assert.DoesNotContain(service.ListarActividadesDeMascota(mascota.Id), actividad => actividad.CitaId == cita.Id);
    }

    [Fact]
    public void AgregarRecordatorio_DeberiaAsignarloALaMascota()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var fecha = new DateTime(2026, 8, 30);

        var recordatorio = service.AgregarRecordatorio(mascota.Id, "Pastilla antipulgas", fecha, "8:00 AM");

        Assert.NotNull(recordatorio.Id);
        Assert.Equal(mascota.Id, recordatorio.MascotaId);
        Assert.Equal(fecha, recordatorio.Fecha);
        Assert.Contains(recordatorio, service.ListarRecordatoriosDeMascota(mascota.Id));
    }

    [Fact]
    public void ListarProximosRecordatoriosDeCliente_DeberiaDevolverSoloLosDeSusMascotas()
    {
        var service = CrearServicio();
        var zeus = ObtenerZeus(service);
        var otraMascota = service.ListarMascotas().First(mascota => mascota.ClienteId != zeus.ClienteId);

        var recordatorioZeus = service.AgregarRecordatorio(zeus.Id, "Pastilla", DateTime.Today.AddDays(1), "8:00 AM");
        var recordatorioOtro = service.AgregarRecordatorio(otraMascota.Id, "Otra pastilla", DateTime.Today.AddDays(1), "9:00 AM");

        var recordatorios = service.ListarProximosRecordatoriosDeCliente(zeus.ClienteId);

        Assert.Contains(recordatorioZeus, recordatorios);
        Assert.DoesNotContain(recordatorioOtro, recordatorios);
        Assert.All(recordatorios, recordatorio => Assert.NotEqual(otraMascota.Id, recordatorio.MascotaId));
    }

    [Fact]
    public void EliminarRecordatorio_DeberiaQuitarlo()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);
        var recordatorio = service.AgregarRecordatorio(mascota.Id, "Pastilla", DateTime.Today.AddDays(2), "8:00 AM");

        Assert.True(service.EliminarRecordatorio(recordatorio.Id));
        Assert.DoesNotContain(recordatorio, service.ListarRecordatoriosDeMascota(mascota.Id));
        Assert.False(service.EliminarRecordatorio(recordatorio.Id));
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

        var evaluacion = service.AgregarEvaluacion(mascota.Id, 8, 9, 7, 10, "Evaluación de prueba.");

        Assert.NotNull(evaluacion.Id);
        Assert.Equal(mascota.Id, evaluacion.MascotaId);
        Assert.Equal(8, evaluacion.Comportamiento);
        Assert.Equal(9, evaluacion.Higiene);
        Assert.Equal(7, evaluacion.Movimiento);
        Assert.Equal(10, evaluacion.Animo);
        Assert.Equal("Evaluación de prueba.", evaluacion.Comentario);
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
            service.AgregarEvaluacion(mascota.Id, puntaje, 5, 5, 5, "Comentario"));

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
    public void ObtenerTimelineDeMascota_DeberiaCombinarYOrdenarLosEventos()
    {
        var service = CrearServicio();
        var mascota = ObtenerZeus(service);

        var consulta = service.AgregarConsulta(mascota.Id, "Control general", "Sin novedades.");
        var evaluacion = service.AgregarEvaluacion(mascota.Id, 8, 9, 7, 10, "Evaluación adicional.");

        var timeline = service.ObtenerTimelineDeMascota(mascota.Id);

        Assert.Contains(timeline, evento => evento.Tipo == "Consulta" && evento.Titulo == consulta.Motivo && evento.Detalle == consulta.Notas);
        Assert.Contains(timeline, evento => evento.Tipo == "Evaluacion" && evento.Titulo == $"Evaluación: {evaluacion.Promedio}/10");
        Assert.Contains(timeline, evento => evento.Tipo == "Peso" && evento.Titulo.EndsWith(" kg"));
        Assert.Contains(timeline, evento => evento.Tipo == "Actividad" && evento.Titulo == "Walking");
        Assert.Equal(timeline.OrderByDescending(evento => evento.Fecha), timeline);
    }

    [Fact]
    public void Notificaciones_DeberianCrearFiltrarYMarcarseComoLeidas()
    {
        var service = CrearServicio();
        var cliente = service.AgregarCliente("Cliente de prueba", 30, "3000000000", "prueba@email.com", "Calle de prueba");
        var mascota = service.AgregarMascota("Mascota de prueba", "Perro", "Mestizo", 2, cliente.Id, "", "");
        var clienteId = mascota.ClienteId;

        var evaluacion = service.AgregarEvaluacion(mascota.Id, 8, 9, 7, 10, "Evaluación de prueba.");
        var consulta = service.AgregarConsulta(mascota.Id, "Control", "Todo bien.");
        var otraNotificacion = service.CrearNotificacion("otro-cliente", "Mensaje privado", "Cita");

        var notificaciones = service.ListarNotificacionesDeCliente(clienteId);

        Assert.Equal(2, notificaciones.Count);
        Assert.Equal(2, service.ContarNoLeidas(clienteId));
        Assert.Contains(notificaciones, notificacion => notificacion.Tipo == "Evaluacion" && notificacion.Mensaje.Contains(mascota.Nombre));
        Assert.Contains(notificaciones, notificacion => notificacion.Tipo == "Consulta" && notificacion.Mensaje.Contains(mascota.Nombre));
        Assert.DoesNotContain(notificaciones, notificacion => notificacion.Id == otraNotificacion.Id);

        Assert.True(service.MarcarComoLeida(notificaciones[0].Id));
        Assert.Equal(1, service.ContarNoLeidas(clienteId));

        service.MarcarTodasComoLeidas(clienteId);

        Assert.Equal(0, service.ContarNoLeidas(clienteId));
        Assert.True(notificaciones.All(notificacion => notificacion.Leida));
        Assert.NotNull(evaluacion);
        Assert.NotNull(consulta);
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
