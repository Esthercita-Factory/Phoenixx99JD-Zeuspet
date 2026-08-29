using Phoenixx99JD_Zeuspet.Models;
using System.Text;

namespace Phoenixx99JD_Zeuspet.Services;

public class VeterinariaService
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Mascota> _mascotas = [];

    // Diccionario que asocia el ID del cliente con su objeto Cliente,
    // permitiendo un acceso directo (O(1)) sin recorrer toda la lista.
    private readonly Dictionary<string, Cliente> _clientesPorId = [];

    // ============================================
    // CRUD BASICO (agregar, modificar, eliminar)
    // ============================================

    public Cliente AgregarCliente(string nombre, string telefono, string email, string direccion, int edad = 0)
    {
        var cliente = new Cliente(nombre, telefono, email, direccion, edad);
        _clientes.Add(cliente);
        _clientesPorId[cliente.Id] = cliente;
        return cliente;
    }

    public List<Cliente> ListarClientes() => _clientes;

    // Busqueda lineal con LINQ: recorre la lista hasta encontrar la coincidencia.
    public Cliente? BuscarClientePorId(string id) => _clientes.FirstOrDefault(c => c.Id == id);

    // Acceso directo usando el diccionario, mas rapido que FirstOrDefault.
    public Cliente? ObtenerClientePorIdRapido(string id) => _clientesPorId.GetValueOrDefault(id);

    // Where: filtra clientes cuyo nombre contenga el texto buscado.
    public List<Cliente> BuscarClientesPorNombre(string texto)
    {
        return _clientes.Where(c => c.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Modifica los datos de un cliente existente dentro de la lista.
    public bool ModificarCliente(string id, string nombre, string telefono, string email, string direccion, int? edad = null)
    {
        var cliente = BuscarClientePorId(id);
        if (cliente == null) return false;

        cliente.Nombre = nombre.Trim();
        cliente.Telefono = telefono.Trim();
        cliente.Email = email.Trim();
        cliente.Direccion = direccion.Trim();
        if (edad.HasValue) cliente.Edad = edad.Value;
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

    public Mascota AgregarMascota(string nombre, string especie, string raza, int edad, string clienteId)
    {
        var cliente = BuscarClientePorId(clienteId);
        if (cliente == null)
            throw new InvalidOperationException("El cliente no existe.");

        var mascota = new Mascota(nombre, especie, raza, edad, clienteId);
        _mascotas.Add(mascota);
        cliente.Mascotas.Add(mascota);
        return mascota;
    }

    public List<Mascota> ListarMascotas() => _mascotas;

    // Where: filtra las mascotas que pertenecen a un cliente.
    public List<Mascota> ListarMascotasDeCliente(string clienteId)
    {
        return _mascotas.Where(m => m.ClienteId == clienteId).ToList();
    }

    public Mascota? BuscarMascotaPorId(string id) => _mascotas.FirstOrDefault(m => m.Id == id);

    // Modifica los datos de una mascota existente dentro de la lista.
    public bool ModificarMascota(string id, string nombre, string especie, string raza, int edad)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;

        mascota.Nombre = nombre.Trim();
        mascota.Especie = especie.Trim();
        mascota.Raza = raza.Trim();
        mascota.Edad = edad;
        return true;
    }

    public bool EliminarMascota(string id)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;
        _mascotas.Remove(mascota);
        BuscarClientePorId(mascota.ClienteId)?.Mascotas.Remove(mascota);
        return true;
    }

    // ============================================
    // LINQ - SINTAXIS DE METODOS
    // ============================================

    // Where: filtra los clientes que tienen al menos una mascota de la especie indicada.
    public List<Cliente> FiltrarClientesPorEspecie(string especie)
    {
        return _clientes
            .Where(c => c.Mascotas.Any(m => m.Especie == especie))
            .ToList();
    }

    // Where: filtra las mascotas cuya edad sea mayor o igual a la minima indicada.
    public List<Mascota> FiltrarMascotasPorEdad(int edadMinima)
    {
        return _mascotas
            .Where(m => m.Edad >= edadMinima)
            .ToList();
    }

    // Select: proyecta unicamente los nombres de los clientes.
    public List<string> ObtenerNombresClientes()
    {
        return _clientes
            .Select(c => c.Nombre)
            .ToList();
    }

    // OrderBy: ordena las mascotas alfabeticamente por nombre.
    public List<Mascota> OrdenarMascotasPorNombre()
    {
        return _mascotas
            .OrderBy(m => m.Nombre)
            .ToList();
    }

    // OrderByDescending: ordena las mascotas de mayor a menor edad.
    public List<Mascota> OrdenarMascotasPorEdadDescendente()
    {
        return _mascotas
            .OrderByDescending(m => m.Edad)
            .ToList();
    }

    // GroupBy: agrupa las mascotas por especie y cuenta cuantas hay de cada una.
    public Dictionary<string, int> ContarMascotasPorEspecie()
    {
        return _mascotas
            .GroupBy(m => m.Especie)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // First: devuelve el primer elemento de la coleccion.
    public Mascota ObtenerPrimeraMascota()
    {
        return _mascotas.First();
    }

    // FirstOrDefault: devuelve el primer elemento que cumpla la condicion, o null si no hay ninguno.
    public Mascota? ObtenerPrimerPerro()
    {
        return _mascotas.FirstOrDefault(m => m.Especie == "Perro");
    }

    // Any: indica si existe al menos un elemento que cumpla la condicion.
    public bool ExisteAlgunaMascotaDeEspecie(string especie)
    {
        return _mascotas.Any(m => m.Especie == especie);
    }

    // All: indica si todos los elementos cumplen la condicion.
    public bool TodasLasMascotasTienenRaza()
    {
        return _mascotas.All(m => !string.IsNullOrWhiteSpace(m.Raza));
    }

    // Count: cuenta cuantos elementos cumplen la condicion.
    public int ContarMascotasDeEspecie(string especie)
    {
        return _mascotas.Count(m => m.Especie == especie);
    }

    // ============================================
    // LINQ - SINTAXIS DE CONSULTA (query syntax)
    // ============================================

    // Equivalente a FiltrarClientesPorEspecie, pero con sintaxis de consulta.
    public List<Cliente> FiltrarClientesPorEspecieQuery(string especie)
    {
        var resultado = from c in _clientes
                        where c.Mascotas.Any(m => m.Especie == especie)
                        select c;
        return resultado.ToList();
    }

    // Equivalente a OrdenarMascotasPorEdadDescendente con sintaxis de consulta.
    public List<Mascota> OrdenarMascotasPorEdadDescendenteQuery()
    {
        var resultado = from m in _mascotas
                        orderby m.Edad descending
                        select m;
        return resultado.ToList();
    }

    // Equivalente a un Select con sintaxis de consulta: proyecta los nombres de las mascotas.
    public List<string> ObtenerNombresMascotasQuery()
    {
        var resultado = from m in _mascotas
                        select m.Nombre;
        return resultado.ToList();
    }

    // ============================================
    // LINQ - CONSULTAS ENCADENADAS
    // ============================================

    // Encadena Where + OrderBy + Select en una sola consulta:
    // 1) filtra solo los perros, 2) los ordena por edad ascendente y
    // 3) proyecta el nombre y telefono del dueno junto con la edad de la mascota.
    public List<(string Dueno, string Telefono, int EdadMascota)> ObtenerDuenosDePerrosOrdenadosPorEdad()
    {
        return _mascotas
            .Where(m => m.Especie == "Perro")
            .OrderBy(m => m.Edad)
            .Select(m => (Dueno: ObtenerClientePorIdRapido(m.ClienteId)?.Nombre ?? "N/A",
                          Telefono: ObtenerClientePorIdRapido(m.ClienteId)?.Telefono ?? "N/A",
                          EdadMascota: m.Edad))
            .ToList();
    }

    // ============================================
    // LINQ - PROBLEMAS PRACTICOS
    // ============================================

    // Mascota mas joven: se ordena por edad ascendente y se toma el primer elemento.
    public Mascota ObtenerMascotaMasJoven()
    {
        return _mascotas.OrderBy(m => m.Edad).First();
    }

    // Mascota de mayor edad: se ordena por edad descendente y se toma el primer elemento.
    public Mascota ObtenerMascotaDeMayorEdad()
    {
        return _mascotas.OrderByDescending(m => m.Edad).First();
    }

    // Any: verifica si existe al menos una mascota sin raza definida (campo vacio).
    public bool ExisteMascotaSinRaza()
    {
        return _mascotas.Any(m => string.IsNullOrWhiteSpace(m.Raza));
    }

    // Select + OrderBy: proyecta los nombres de los clientes en mayusculas
    // y los ordena alfabeticamente.
    public List<string> ObtenerNombresClientesEnMayusculasOrdenados()
    {
        return _clientes
            .Select(c => c.Nombre.ToUpperInvariant())
            .OrderBy(nombre => nombre)
            .ToList();
    }

    public void CargarDatosEjemplo()
    {
        var c1 = AgregarCliente("Carlos Mendoza", "3001234567", "carlos@email.com", "Calle 45 #12-30", 45);
        var c2 = AgregarCliente("Ana Lucia Torres", "3109876543", "ana.torres@email.com", "Carrera 15 #80-22", 32);
        var c3 = AgregarCliente("Roberto Jimenez", "3205551234", "roberto.j@email.com", "Av. Siempre Viva #34", 50);
        var c4 = AgregarCliente("Maria Fernanda Ruiz", "3157778899", "mfr@email.com", "Calle 100 #20-15", 28);
        var c5 = AgregarCliente("Pedro Gomez", "3014445566", "pedro.gomez@email.com", "Transversal 8 #45-60", 39);

        AgregarMascota("Zeus", "Perro", "Pastor Aleman", 4, c1.Id);
        AgregarMascota("Luna", "Gato", "Siames", 2, c1.Id);
        AgregarMascota("Rocky", "Perro", "Bulldog Frances", 3, c2.Id);
        AgregarMascota("Mimi", "Gato", "Persa", 5, c2.Id);
        AgregarMascota("Max", "Perro", "Golden Retriever", 6, c3.Id);
        AgregarMascota("Coco", "Perro", "Chihuahua", 1, c4.Id);
        AgregarMascota("Pelusa", "Gato", "Angora", 3, c4.Id);
        AgregarMascota("Toby", "Perro", "Labrador", 7, c5.Id);
        AgregarMascota("Nina", "Conejo", "Mini Lop", 2, c5.Id);
        // Mascota sin raza definida, para poder probar la consulta Any/All.
        AgregarMascota("Sombra", "Gato", "", 4, c5.Id);
    }

    // ============================================
    // PROGRAMACION ASINCRONA (async / await, Task)
    // ============================================

    // Simula un proceso de la clinica (consulta, examen, analisis) que toma
    // un tiempo determinado. Gracias a Task.Delay no bloquea el hilo principal.
    private static async Task<string> SimularProcesoAsync(string nombre, int milisegundos)
    {
        await Task.Delay(milisegundos);
        return $"Proceso '{nombre}' finalizado despues de {milisegundos} ms.";
    }

    // Genera varias secciones del reporte EN PARALELO. Task.WhenAll espera
    // hasta que TODAS las tareas concluyan y agrupa sus resultados.
    public async Task<IReadOnlyList<string>> GenerarReporteParaleloAsync()
    {
        Task<string> consultas = SimularProcesoAsync("Consultas", 300);
        Task<string> vacunaciones = SimularProcesoAsync("Vacunaciones", 500);
        Task<string> seguimientos = SimularProcesoAsync("Seguimientos", 200);

        return await Task.WhenAll(consultas, vacunaciones, seguimientos);
    }

    // Ejecuta varios procesos de diagnostico en paralelo y se queda con el
    // PRIMERO en finalizar mediante Task.WhenAny. Retorna esa unica tarea.
    public async Task<(string Etiqueta, string Resultado)> EjecutarDiagnosticoRapidoAsync()
    {
        Task<string> consulta = SimularProcesoAsync("Consulta general", 500);
        Task<string> analisis = SimularProcesoAsync("Analisis de sangre", 200);
        Task<string> imagen = SimularProcesoAsync("Imagen radiografica", 400);

        Task<string> masRapido = await Task.WhenAny(consulta, analisis, imagen);
        return ("Diagnostico mas rapido", await masRapido);
    }

    // Escribe el reporte de la clinica en un archivo usando E/S asincrona,
    // de modo que el hilo principal quede libre mientras se guarda.
    public async Task<string> GuardarReporteAsync(string rutaArchivo)
    {
        IReadOnlyList<string> secciones = await GenerarReporteParaleloAsync();

        var builder = new StringBuilder();
        builder.AppendLine("=== REPORTE SEMANAL - CLINICA VETERINARIA ZEUSPET ===");
        builder.AppendLine($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        builder.AppendLine($"Clientes registrados: {_clientes.Count}");
        builder.AppendLine($"Mascotas registradas: {_mascotas.Count}");
        builder.AppendLine();
        foreach (string seccion in secciones)
            builder.AppendLine(seccion);
        builder.AppendLine();
        builder.AppendLine("=== FIN DEL REPORTE ===");

        await File.WriteAllTextAsync(rutaArchivo, builder.ToString());
        return rutaArchivo;
    }
}
