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
    private readonly List<Consulta> _consultas = [];
    private readonly List<RegistroPeso> _registrosPeso = [];
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

    public Mascota AgregarMascota(string nombre, string especie, string raza, int edad, string clienteId)
    {
        var cliente = BuscarClientePorId(clienteId);
        if (cliente == null)
            throw new InvalidOperationException("El cliente no existe.");

        var mascota = new Mascota(GenerarId(), nombre, especie, raza, edad, clienteId);
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

        var consulta = new Consulta(GenerarId(), mascotaId, DateTime.Now, motivo, notas);
        _consultas.Add(consulta);
        return consulta;
    }

    public List<Consulta> ListarConsultasDeMascota(string mascotaId)
    {
        return _consultas
            .Where(consulta => consulta.MascotaId == mascotaId)
            .OrderByDescending(consulta => consulta.Fecha)
            .ToList();
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
        int animo)
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
            DateTime.Now);

        _evaluaciones.Add(evaluacion);
        return evaluacion;
    }

    public List<EvaluacionCalidadVida> ListarEvaluacionesDeMascota(string mascotaId)
    {
        return _evaluaciones
            .Where(e => e.MascotaId == mascotaId)
            .OrderByDescending(e => e.Fecha)
            .ToList();
    }

    public EvaluacionCalidadVida? ObtenerUltimaEvaluacion(string mascotaId)
    {
        return _evaluaciones
            .Where(e => e.MascotaId == mascotaId)
            .MaxBy(e => e.Fecha);
    }

    public CitaAgenda AgregarCita(string mascotaId, string titulo, string hora, string tipo)
    {
        ValidarMascotaExiste(mascotaId);

        var cita = new CitaAgenda(GenerarId(), mascotaId, titulo, hora, tipo);
        _citas.Add(cita);
        return cita;
    }

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
            .Where(cita => !cita.Completada)
            .OrderBy(OrdenDeHora)
            .ToList();
    }

    public bool MarcarCitaCompletada(string id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        if (cita == null) return false;

        cita.Completada = true;
        return true;
    }

    public bool EliminarCita(string id)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        return cita != null && _citas.Remove(cita);
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
        return DateTime.TryParse(cita.Hora, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var hora)
            ? hora
            : DateTime.MaxValue;
    }

    private void CargarDatosEjemplo()
    {
        var c1 = AgregarCliente("Carlos Mendoza", 45, "3001234567", "carlos@email.com", "Calle 45 #12-30");
        var c2 = AgregarCliente("Ana Lucia Torres", 32, "3109876543", "ana.torres@email.com", "Carrera 15 #80-22");
        var c3 = AgregarCliente("Roberto Jimenez", 50, "3205551234", "roberto.j@email.com", "Av. Siempre Viva #34");
        var c4 = AgregarCliente("Maria Fernanda Ruiz", 28, "3157778899", "mfr@email.com", "Calle 100 #20-15");
        var c5 = AgregarCliente("Pedro Gomez", 39, "3014445566", "pedro.gomez@email.com", "Transversal 8 #45-60");

        var zeus = AgregarMascota("Zeus", "Perro", "Pastor Aleman", 4, c1.Id);
        var luna = AgregarMascota("Luna", "Gato", "Siames", 2, c1.Id);
        AgregarMascota("Rocky", "Perro", "Bulldog Frances", 3, c2.Id);
        AgregarMascota("Mimi", "Gato", "Persa", 5, c2.Id);
        var max = AgregarMascota("Max", "Perro", "Golden Retriever", 6, c3.Id);
        AgregarMascota("Coco", "Perro", "Chihuahua", 1, c4.Id);
        AgregarMascota("Pelusa", "Gato", "Angora", 3, c4.Id);
        AgregarMascota("Toby", "Perro", "Labrador", 7, c5.Id);
        AgregarMascota("Nina", "Conejo", "Mini Lop", 2, c5.Id);

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

        AgregarCita(zeus.Id, "Veterinary Appointment", "10:00 AM", "veterinaria");
        AgregarCita(_mascotas.First(m => m.Nombre == "Luna").Id, "Grooming", "2:00 PM", "aseo");
        AgregarCita(_mascotas.First(m => m.Nombre == "Max").Id, "Playing & Socializing", "5:00 PM", "social");
    }
}
