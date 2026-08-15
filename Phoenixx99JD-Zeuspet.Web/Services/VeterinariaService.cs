using Phoenixx99JD_Zeuspet.Web.Models;

namespace Phoenixx99JD_Zeuspet.Web.Services;

public class VeterinariaService
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Mascota> _mascotas = [];
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
        BuscarClientePorId(mascota.ClienteId)?.Mascotas.Remove(mascota);
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
            ["especies"] = _mascotas
                .GroupBy(m => m.Especie)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    private static string GenerarId()
    {
        var random = new Random();
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, 8)
            .Select(_ => caracteres[random.Next(caracteres.Length)])
            .ToArray());
    }

    private void CargarDatosEjemplo()
    {
        var c1 = AgregarCliente("Carlos Mendoza", 45, "3001234567", "carlos@email.com", "Calle 45 #12-30");
        var c2 = AgregarCliente("Ana Lucia Torres", 32, "3109876543", "ana.torres@email.com", "Carrera 15 #80-22");
        var c3 = AgregarCliente("Roberto Jimenez", 50, "3205551234", "roberto.j@email.com", "Av. Siempre Viva #34");
        var c4 = AgregarCliente("Maria Fernanda Ruiz", 28, "3157778899", "mfr@email.com", "Calle 100 #20-15");
        var c5 = AgregarCliente("Pedro Gomez", 39, "3014445566", "pedro.gomez@email.com", "Transversal 8 #45-60");

        AgregarMascota("Zeus", "Perro", "Pastor Aleman", 4, c1.Id);
        AgregarMascota("Luna", "Gato", "Siames", 2, c1.Id);
        AgregarMascota("Rocky", "Perro", "Bulldog Frances", 3, c2.Id);
        AgregarMascota("Mimi", "Gato", "Persa", 5, c2.Id);
        AgregarMascota("Max", "Perro", "Golden Retriever", 6, c3.Id);
        AgregarMascota("Coco", "Perro", "Chihuahua", 1, c4.Id);
        AgregarMascota("Pelusa", "Gato", "Angora", 3, c4.Id);
        AgregarMascota("Toby", "Perro", "Labrador", 7, c5.Id);
        AgregarMascota("Nina", "Conejo", "Mini Lop", 2, c5.Id);
    }
}
