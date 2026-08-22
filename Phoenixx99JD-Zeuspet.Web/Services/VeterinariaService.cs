using System.Globalization;
using Phoenixx99JD_Zeuspet.Web.Models;

namespace Phoenixx99JD_Zeuspet.Web.Services;

public class VeterinariaService
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Mascota> _mascotas = [];
    private readonly List<Actividad> _actividades = [];
    private readonly List<EvaluacionCalidadVida> _evaluaciones = [];
    private readonly List<CitaAgenda> _citas = [];
    private readonly List<Recordatorio> _recordatorios = [];
    private readonly List<Consulta> _consultas = [];
    private readonly List<RegistroPeso> _registrosPeso = [];
    private readonly List<Notificacion> _notificaciones = [];
    private readonly List<PublicacionComunidad> _publicaciones = [];
    private readonly List<ComentarioComunidad> _comentariosComunidad = [];
    private readonly Dictionary<string, Cliente> _clientesPorId = [];

    public VeterinariaService()
    {
        CargarDatosEjemplo();
    }

    public List<Cliente> ListarClientes() => _clientes;

    public Cliente? BuscarClientePorId(string id) => _clientesPorId.GetValueOrDefault(id);

    public Cliente AgregarCliente(string nombre, int edad, string telefono, string email, string direccion)
    {
        var cliente = new Cliente(GenerarId(), nombre, edad, telefono, email, direccion);
        _clientes.Add(cliente);
        _clientesPorId[cliente.Id] = cliente;
        return cliente;
    }

    public bool ModificarCliente(string id, string nombre, int edad, string telefono, string email, string direccion)
    {
        var cliente = BuscarClientePorId(id);
        if (cliente == null) return false;

        cliente.Nombre = nombre;
        cliente.Edad = edad;
        cliente.Telefono = telefono;
        cliente.Email = email;
        cliente.Direccion = direccion;
        return true;
    }

    public bool EliminarCliente(string id)
    {
        var cliente = BuscarClientePorId(id);
        if (cliente == null) return false;

        _mascotas.RemoveAll(m => m.ClienteId == id);
        _notificaciones.RemoveAll(notificacion => notificacion.ClienteId == id);
        _clientes.Remove(cliente);
        _clientesPorId.Remove(id);
        return true;
    }

    public List<Mascota> ListarMascotas() => _mascotas;

    public List<Mascota> ListarMascotasDeCliente(string clienteId)
    {
        return _mascotas.Where(m => m.ClienteId == clienteId).ToList();
    }

    public Mascota? BuscarMascotaPorId(string id) => _mascotas.FirstOrDefault(m => m.Id == id);

    public bool ExisteNumeroIdentificacion(string numero)
    {
        return _mascotas.Any(m => string.Equals(m.NumeroIdentificacion, numero, StringComparison.OrdinalIgnoreCase));
    }

    public Mascota AgregarMascota(string nombre, string especie, string raza, int edad, string clienteId, string numeroIdentificacion, string fotoUrl)
    {
        var cliente = BuscarClientePorId(clienteId);
        if (cliente == null)
            throw new InvalidOperationException("El cliente no existe.");

        if (ExisteNumeroIdentificacion(numeroIdentificacion))
            throw new InvalidOperationException("El número de identificación ya está registrado.");

        var mascota = new Mascota(GenerarId(), nombre, especie, raza, edad, clienteId)
        {
            NumeroIdentificacion = numeroIdentificacion,
            FotoUrl = fotoUrl
        };

        _mascotas.Add(mascota);
        cliente.Mascotas.Add(mascota);
        return mascota;
    }

    public bool ModificarMascota(string id, string nombre, string especie, string raza, int edad)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;

        mascota.Nombre = nombre;
        mascota.Especie = especie;
        mascota.Raza = raza;
        mascota.Edad = edad;
        return true;
    }

    public bool EliminarMascota(string id)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;

        _mascotas.Remove(mascota);
        _actividades.RemoveAll(a => a.MascotaId == id);
        _evaluaciones.RemoveAll(e => e.MascotaId == id);
        _citas.RemoveAll(c => c.MascotaId == id);
        _recordatorios.RemoveAll(r => r.MascotaId == id);
        _consultas.RemoveAll(c => c.MascotaId == id);
        _registrosPeso.RemoveAll(r => r.MascotaId == id);
        BuscarClientePorId(mascota.ClienteId)?.Mascotas.Remove(mascota);
        return true;
    }

    public bool ActualizarDatosMascota(string id, double? peso, string sexo, string estado, string notas, string fotoUrl)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;

        mascota.Peso = peso;
        mascota.Sexo = sexo;
        mascota.Estado = estado;
        mascota.Notas = notas;
        mascota.FotoUrl = fotoUrl;
        return true;
    }

    public Consulta AgregarConsulta(string mascotaId, string motivo, string notas)
    {
        ValidarMascotaExiste(mascotaId);

        var mascota = BuscarMascotaPorId(mascotaId)!;
        var consulta = new Consulta(GenerarId(), mascotaId, DateTime.Now, motivo, notas);
        _consultas.Add(consulta);
        CrearNotificacion(mascota.ClienteId, $"Nueva consulta registrada para {mascota.Nombre}", "Consulta");
        return consulta;
    }

    public List<Consulta> ListarConsultasDeMascota(string mascotaId)
    {
        return _consultas
            .Where(consulta => consulta.MascotaId == mascotaId)
            .OrderByDescending(consulta => consulta.Fecha)
            .ToList();
    }

    public Notificacion CrearNotificacion(string clienteId, string mensaje, string tipo)
    {
        var notificacion = new Notificacion(GenerarId(), clienteId, mensaje, DateTime.Now, tipo);
        _notificaciones.Add(notificacion);
        return notificacion;
    }

    public List<Notificacion> ListarNotificacionesDeCliente(string clienteId)
    {
        return _notificaciones
            .Where(notificacion => notificacion.ClienteId == clienteId)
            .OrderByDescending(notificacion => notificacion.Fecha)
            .ToList();
    }

    public int ContarNoLeidas(string clienteId) =>
        _notificaciones.Count(notificacion => notificacion.ClienteId == clienteId && !notificacion.Leida);

    public bool MarcarComoLeida(string id)
    {
        var notificacion = _notificaciones.FirstOrDefault(item => item.Id == id);
        if (notificacion is null) return false;

        notificacion.Leida = true;
        return true;
    }

    public void MarcarTodasComoLeidas(string clienteId)
    {
        foreach (var notificacion in _notificaciones.Where(item => item.ClienteId == clienteId))
            notificacion.Leida = true;
    }

    public Actividad AgregarActividad(string mascotaId, string nombre, string hora, string grupo, DateTime fecha)
    {
        ValidarMascotaExiste(mascotaId);

        var actividad = new Actividad(GenerarId(), mascotaId, nombre, hora, grupo, fecha);
        _actividades.Add(actividad);
        return actividad;
    }

    public List<Actividad> ListarActividadesDeMascota(string mascotaId)
    {
        return _actividades.Where(a => a.MascotaId == mascotaId).ToList();
    }

    public List<Actividad> ListarTodasLasActividades() => _actividades.ToList();

    public Recordatorio AgregarRecordatorio(string mascotaId, string titulo, DateTime fecha, string hora)
    {
        ValidarMascotaExiste(mascotaId);

        var recordatorio = new Recordatorio(GenerarId(), mascotaId, titulo, fecha, hora);
        _recordatorios.Add(recordatorio);
        return recordatorio;
    }

    public List<Recordatorio> ListarRecordatoriosDeMascota(string mascotaId) =>
        _recordatorios
            .Where(recordatorio => recordatorio.MascotaId == mascotaId)
            .OrderBy(OrdenDeRecordatorio)
            .ToList();

    public List<Recordatorio> ListarProximosRecordatoriosDeCliente(string clienteId)
    {
        var mascotasDelCliente = ListarMascotasDeCliente(clienteId).Select(m => m.Id).ToHashSet();
        return _recordatorios
            .Where(recordatorio => mascotasDelCliente.Contains(recordatorio.MascotaId))
            .OrderBy(OrdenDeRecordatorio)
            .ToList();
    }

    public bool EliminarRecordatorio(string id)
    {
        var recordatorio = _recordatorios.FirstOrDefault(item => item.Id == id);
        return recordatorio != null && _recordatorios.Remove(recordatorio);
    }

    public RegistroPeso AgregarRegistroPeso(string mascotaId, double peso, DateTime fecha)
    {
        ValidarMascotaExiste(mascotaId);

        var registro = new RegistroPeso(GenerarId(), mascotaId, peso, fecha);
        _registrosPeso.Add(registro);
        return registro;
    }

    public List<RegistroPeso> ListarRegistroPesoDeMascota(string mascotaId)
    {
        return _registrosPeso
            .Where(registro => registro.MascotaId == mascotaId)
            .OrderBy(registro => registro.Fecha)
            .ToList();
    }

    public EvaluacionCalidadVida AgregarEvaluacion(
        string mascotaId,
        int comportamiento,
        int higiene,
        int movimiento,
        int animo,
        string comentario = "")
    {
        ValidarPuntaje(comportamiento, nameof(comportamiento));
        ValidarPuntaje(higiene, nameof(higiene));
        ValidarPuntaje(movimiento, nameof(movimiento));
        ValidarPuntaje(animo, nameof(animo));

        ValidarMascotaExiste(mascotaId);

        var evaluacion = new EvaluacionCalidadVida(
            GenerarId(),
            mascotaId,
            comportamiento,
            higiene,
            movimiento,
            animo,
            DateTime.Now,
            comentario);

        _evaluaciones.Add(evaluacion);
        var mascota = BuscarMascotaPorId(mascotaId)!;
        CrearNotificacion(mascota.ClienteId, $"Nueva evaluación registrada para {mascota.Nombre}", "Evaluacion");
        return evaluacion;
    }

    public List<EvaluacionCalidadVida> ListarEvaluacionesDeMascota(string mascotaId)
    {
        return _evaluaciones
            .Where(e => e.MascotaId == mascotaId)
            .OrderByDescending(e => e.Fecha)
            .ToList();
    }

    public List<EventoTimeline> ObtenerTimelineDeMascota(string mascotaId)
    {
        var eventos = ListarConsultasDeMascota(mascotaId)
            .Select(consulta => new EventoTimeline(
                consulta.Fecha,
                "Consulta",
                consulta.Motivo,
                consulta.Notas))
            .Concat(ListarEvaluacionesDeMascota(mascotaId).Select(evaluacion => new EventoTimeline(
                evaluacion.Fecha,
                "Evaluacion",
                $"Evaluación: {evaluacion.Promedio}/10",
                $"Comportamiento: {evaluacion.Comportamiento} · Higiene: {evaluacion.Higiene} · Movimiento: {evaluacion.Movimiento} · Ánimo: {evaluacion.Animo}")))
            .Concat(ListarRegistroPesoDeMascota(mascotaId).Select(registro => new EventoTimeline(
                registro.Fecha,
                "Peso",
                $"{registro.Peso} kg",
                "Registro de peso")))
            .Concat(ListarActividadesDeMascota(mascotaId).Select(actividad => new EventoTimeline(
                actividad.Fecha,
                "Actividad",
                actividad.Nombre,
                $"{actividad.Hora} · {actividad.Grupo}")))
            .OrderByDescending(evento => evento.Fecha)
            .ToList();

        return eventos;
    }

    public EvaluacionCalidadVida? ObtenerUltimaEvaluacion(string mascotaId)
    {
        return _evaluaciones
            .Where(e => e.MascotaId == mascotaId)
            .MaxBy(e => e.Fecha);
    }

    public CitaAgenda AgregarCita(string mascotaId, string titulo, string hora, string tipo, string estado = "Confirmada", DateTime fecha = default)
    {
        ValidarMascotaExiste(mascotaId);

        var cita = new CitaAgenda(GenerarId(), mascotaId, titulo, hora, tipo, fecha)
        {
            Estado = estado
        };
        _citas.Add(cita);

        if (estado == "Confirmada")
            CrearActividadDesdeCita(cita);

        return cita;
    }

    public CitaAgenda SolicitarCita(string mascotaId, string titulo, string hora, string tipo, DateTime fecha = default) =>
        AgregarCita(mascotaId, titulo, hora, tipo, "Pendiente", fecha);

    public List<CitaAgenda> ListarCitasDeMascota(string mascotaId)
    {
        return _citas
            .Where(cita => cita.MascotaId == mascotaId)
            .OrderBy(OrdenDeHora)
            .ToList();
    }

    public List<CitaAgenda> ListarProximasCitas()
    {
        return _citas
            .Where(cita => !cita.Completada && cita.Estado != "Rechazada")
            .OrderBy(OrdenDeHora)
            .ToList();
    }

    public List<CitaAgenda> ListarCitasPendientes()
    {
        return _citas
            .Where(cita => !cita.Completada && cita.Estado == "Pendiente")
            .OrderBy(OrdenDeHora)
            .ToList();
    }

    public bool ConfirmarCita(string id) => ActualizarEstadoCita(id, "Confirmada", "confirmada");

    public bool RechazarCita(string id) => ActualizarEstadoCita(id, "Rechazada", "rechazada");

    public bool MarcarCitaCompletada(string id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        if (cita == null) return false;

        cita.Completada = true;
        return true;
    }

    private bool ActualizarEstadoCita(string id, string estado, string estadoMensaje)
    {
        var cita = _citas.FirstOrDefault(item => item.Id == id);
        if (cita is null) return false;

        cita.Estado = estado;
        if (estado == "Confirmada")
            CrearActividadDesdeCita(cita);
        else
            EliminarActividadDeCita(cita.Id);

        var mascota = BuscarMascotaPorId(cita.MascotaId);
        if (mascota is not null)
        {
            CrearNotificacion(
                mascota.ClienteId,
                $"La cita \"{cita.Titulo}\" para {mascota.Nombre} fue {estadoMensaje}.",
                "Cita");
        }

        return true;
    }

    public bool EliminarCita(string id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        if (cita == null) return false;

        EliminarActividadDeCita(cita.Id);
        return _citas.Remove(cita);
    }

    private void CrearActividadDesdeCita(CitaAgenda cita)
    {
        if (_actividades.Any(actividad => actividad.CitaId == cita.Id))
            return;

        _actividades.Add(new Actividad(
            GenerarId(),
            cita.MascotaId,
            cita.Titulo,
            cita.Hora,
            "Alone",
            cita.Fecha,
            cita.Id));
    }

    private void EliminarActividadDeCita(string citaId) =>
        _actividades.RemoveAll(actividad => actividad.CitaId == citaId);

    public PublicacionComunidad CrearPublicacion(
        string clienteId,
        string? mascotaId,
        string contenido,
        string categoria,
        bool esVeterinario)
    {
        var publicacion = new PublicacionComunidad(
            GenerarId(),
            clienteId,
            mascotaId,
            contenido,
            categoria,
            DateTime.Now,
            esVeterinario);

        _publicaciones.Add(publicacion);
        return publicacion;
    }

    public List<PublicacionComunidad> ListarPublicaciones() =>
        _publicaciones
            .OrderByDescending(publicacion => publicacion.Fecha)
            .ToList();

    public List<PublicacionComunidad> ListarPublicacionesPorCategoria(string categoria) =>
        _publicaciones
            .Where(publicacion => string.Equals(publicacion.Categoria, categoria, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(publicacion => publicacion.Fecha)
            .ToList();

    public int ToggleLike(string publicacionId, string clienteId)
    {
        var publicacion = BuscarPublicacion(publicacionId);
        if (publicacion is null)
            throw new InvalidOperationException("La publicación no existe.");

        if (!publicacion.LikesDe.Remove(clienteId))
            publicacion.LikesDe.Add(clienteId);

        return publicacion.LikesDe.Count;
    }

    public ComentarioComunidad AgregarComentario(string publicacionId, string clienteId, string contenido)
    {
        if (BuscarPublicacion(publicacionId) is null)
            throw new InvalidOperationException("La publicación no existe.");

        var comentario = new ComentarioComunidad(
            GenerarId(),
            publicacionId,
            clienteId,
            contenido,
            DateTime.Now);

        _comentariosComunidad.Add(comentario);
        return comentario;
    }

    public List<ComentarioComunidad> ListarComentarios(string publicacionId) =>
        _comentariosComunidad
            .Where(comentario => comentario.PublicacionId == publicacionId)
            .OrderBy(comentario => comentario.Fecha)
            .ToList();

    public bool EliminarPublicacion(string id)
    {
        var publicacion = BuscarPublicacion(id);
        if (publicacion is null) return false;

        _publicaciones.Remove(publicacion);
        _comentariosComunidad.RemoveAll(comentario => comentario.PublicacionId == id);
        return true;
    }

    public string AtenderServicio(string tipo, string nombreMascota)
    {
        ServicioVeterinario servicio = tipo switch
        {
            "ConsultaGeneral" => new ConsultaGeneral(),
            "Vacunacion" => new Vacunacion(),
            _ => throw new InvalidOperationException("Tipo de servicio no valido.")
        };
        return servicio.Atender(nombreMascota);
    }

    public Dictionary<string, object> ObtenerEstadisticas()
    {
        return new Dictionary<string, object>
        {
            ["clientes"] = _clientes.Count,
            ["mascotas"] = _mascotas.Count,
            ["actividades"] = _actividades.Count,
            ["especies"] = _mascotas
                .GroupBy(m => m.Especie)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    private static string GenerarId() => Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

    private PublicacionComunidad? BuscarPublicacion(string id) =>
        _publicaciones.FirstOrDefault(publicacion => publicacion.Id == id);

    private void ValidarMascotaExiste(string mascotaId)
    {
        if (BuscarMascotaPorId(mascotaId) is null)
            throw new InvalidOperationException("La mascota no existe.");
    }

    private static void ValidarPuntaje(int puntaje, string nombre)
    {
        if (puntaje is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nombre, "El puntaje debe estar entre 1 y 10.");
    }

    private static DateTime OrdenDeHora(CitaAgenda cita)
    {
        var fecha = cita.Fecha.Date;
        return fecha.Add(OrdenDeHoraTexto(cita.Hora).TimeOfDay);
    }

    private static DateTime OrdenDeRecordatorio(Recordatorio recordatorio) =>
        recordatorio.Fecha.Date.Add(OrdenDeHoraTexto(recordatorio.Hora).TimeOfDay);

    private static DateTime OrdenDeHoraTexto(string hora) =>
        DateTime.TryParse(hora, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var horaParseada)
            ? horaParseada
            : new DateTime(23, 59, 59);

    private void CargarDatosEjemplo()
    {
        var c1 = AgregarCliente("Carlos Mendoza", 45, "3001234567", "carlos@email.com", "Calle 45 #12-30");
        var c2 = AgregarCliente("Ana Lucia Torres", 32, "3109876543", "ana.torres@email.com", "Carrera 15 #80-22");
        var c3 = AgregarCliente("Roberto Jimenez", 50, "3205551234", "roberto.j@email.com", "Av. Siempre Viva #34");
        var c4 = AgregarCliente("Maria Fernanda Ruiz", 28, "3157778899", "mfr@email.com", "Calle 100 #20-15");
        var c5 = AgregarCliente("Pedro Gomez", 39, "3014445566", "pedro.gomez@email.com", "Transversal 8 #45-60");

        var zeus = AgregarMascota("Zeus", "Perro", "Pastor Aleman", 4, c1.Id, "Z-001", "");
        var luna = AgregarMascota("Luna", "Gato", "Siames", 2, c1.Id, "L-002", "");
        var rocky = AgregarMascota("Rocky", "Perro", "Bulldog Frances", 3, c2.Id, "R-003", "");
        var mimi = AgregarMascota("Mimi", "Gato", "Persa", 5, c2.Id, "M-004", "");
        var max = AgregarMascota("Max", "Perro", "Golden Retriever", 6, c3.Id, "MX-005", "");
        var coco = AgregarMascota("Coco", "Perro", "Chihuahua", 1, c4.Id, "C-006", "");
        var pelusa = AgregarMascota("Pelusa", "Gato", "Angora", 3, c4.Id, "P-007", "");
        var toby = AgregarMascota("Toby", "Perro", "Labrador", 7, c5.Id, "TB-008", "");
        var nina = AgregarMascota("Nina", "Conejo", "Mini Lop", 2, c5.Id, "NN-009", "");

        AgregarEvaluacion(zeus.Id, 9, 10, 9, 10, "Zeus se mostró muy atento y con excelente energía durante la revisión. Recomendamos mantener sus paseos largos.");
        AgregarEvaluacion(luna.Id, 8, 9, 8, 9, "Luna estuvo tranquila en consulta y mantiene un buen estado general. Conviene continuar observando su sensibilidad digestiva.");
        AgregarEvaluacion(rocky.Id, 7, 7, 8, 8, "Rocky se mostró tranquilo durante la consulta. Buen estado general, solo recomendamos vigilar su alimentación las próximas semanas.");
        AgregarEvaluacion(mimi.Id, 9, 8, 7, 9, "Mimi toleró bien la manipulación y se encuentra estable. Sugerimos cuidar su pelaje y mantener controles periódicos.");
        AgregarEvaluacion(max.Id, 8, 8, 6, 8, "Max colaboró muy bien durante la evaluación. Su ánimo es bueno, aunque conviene mantener el control de peso.");
        AgregarEvaluacion(coco.Id, 8, 7, 9, 8, "Coco mostró mucha vitalidad para su edad. Recomendamos vigilar sus articulaciones y conservar rutinas suaves.");
        AgregarEvaluacion(pelusa.Id, 9, 9, 8, 9, "Pelusa estuvo calmada y receptiva. Su condición es favorable; mantener el cepillado frecuente ayudará a su bienestar.");
        AgregarEvaluacion(toby.Id, 6, 8, 7, 8, "Toby se encontró estable y con buen ánimo. Por su edad, aconsejamos revisar movilidad y sostener controles regulares.");
        AgregarEvaluacion(nina.Id, 8, 9, 9, 8, "Nina reaccionó con curiosidad y se mantuvo activa. Su estado general es bueno; ofrecerle espacios seguros para moverse.");

        ActualizarDatosMascota(zeus.Id, 24.5, "Macho", "Activo", "Tiene mucha energía y disfruta los paseos largos.", "");
        ActualizarDatosMascota(luna.Id, 4.2, "Hembra", "Activo", "Sensibilidad digestiva. Prefiere alimento húmedo.", "");
        ActualizarDatosMascota(max.Id, 28.0, "Macho", "Activo", "Control de peso recomendado en la próxima consulta.", "");

        AgregarActividad(zeus.Id, "Walking", "3:00 PM", "Alone", DateTime.Today);
        AgregarActividad(zeus.Id, "Training", "4:00 PM", "Alone", DateTime.Today.AddDays(1));
        AgregarActividad(zeus.Id, "Playdate", "5:00 PM", "With other pets", DateTime.Today.AddDays(2));

        AgregarRegistroPeso(zeus.Id, 22.8, DateTime.Today.AddMonths(-3).AddDays(4));
        AgregarRegistroPeso(zeus.Id, 23.4, DateTime.Today.AddMonths(-2).AddDays(3));
        AgregarRegistroPeso(zeus.Id, 24.0, DateTime.Today.AddMonths(-1).AddDays(8));
        AgregarRegistroPeso(zeus.Id, 24.5, DateTime.Today);
        AgregarRegistroPeso(luna.Id, 3.9, DateTime.Today.AddMonths(-3).AddDays(6));
        AgregarRegistroPeso(luna.Id, 4.0, DateTime.Today.AddMonths(-2).AddDays(10));
        AgregarRegistroPeso(luna.Id, 4.2, DateTime.Today.AddMonths(-1).AddDays(5));

        AgregarCita(zeus.Id, "Veterinary Appointment", "10:00 AM", "veterinaria", "Confirmada", DateTime.Today);
        AgregarCita(_mascotas.First(m => m.Nombre == "Luna").Id, "Grooming", "2:00 PM", "aseo", "Confirmada", DateTime.Today.AddDays(2));
        AgregarCita(_mascotas.First(m => m.Nombre == "Max").Id, "Playing & Socializing", "5:00 PM", "social", "Confirmada", DateTime.Today.AddDays(4));

        AgregarRecordatorio(zeus.Id, "Pastilla antipulgas", DateTime.Today.AddDays(1), "8:00 AM");
        AgregarRecordatorio(luna.Id, "Pastilla desparasitante", DateTime.Today.AddDays(3), "9:30 AM");

        var tipZeus = CrearPublicacion(
            c1.Id,
            zeus.Id,
            "Un paseo corto después de comer ayuda a Zeus a mantenerse activo y tranquilo.",
            "Tip",
            false);
        var preguntaAlimento = CrearPublicacion(
            c2.Id,
            luna.Id,
            "¿Qué alimento húmedo les ha funcionado mejor a sus gatos con sensibilidad digestiva?",
            "Pregunta",
            false);
        CrearPublicacion(
            c3.Id,
            max.Id,
            "Max alcanzó su peso recomendado y volvió a disfrutar sus caminatas largas.",
            "Logro",
            false);
        var anuncioClinica = CrearPublicacion(
            c4.Id,
            null,
            "Este viernes tendremos jornada de vacunación con cupos limitados.",
            "Anuncio",
            true);
        CrearPublicacion(
            c5.Id,
            null,
            "Recordatorio: revisen las uñas y el estado de las almohadillas después de cada paseo.",
            "Tip",
            true);

        tipZeus.LikesDe.Add(c2.Id);
        tipZeus.LikesDe.Add(c3.Id);
        anuncioClinica.LikesDe.Add(c1.Id);
        AgregarComentario(preguntaAlimento.Id, c1.Id, "A Luna le ha funcionado muy bien una fórmula de salmón.");
        AgregarComentario(preguntaAlimento.Id, c4.Id, "Lo ideal es hacer el cambio de alimento gradualmente.");
    }
}
