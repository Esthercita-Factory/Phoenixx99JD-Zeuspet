using Phoenixx99JD_Zeuspet.Console.Models;

namespace Phoenixx99JD_Zeuspet.Console.Services;

public class VeterinariaService
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Mascota> _mascotas = [];
    private int _nextClienteId = 1;
    private int _nextMascotaId = 1;

    public Cliente AgregarCliente(string nombre, string telefono, string email, string direccion)
    {
        var cliente = new Cliente
        {
            Id = _nextClienteId++,
            Nombre = nombre,
            Telefono = telefono,
            Email = email,
            Direccion = direccion
        };
        _clientes.Add(cliente);
        return cliente;
    }

    public List<Cliente> ListarClientes() => _clientes;

    public Cliente? BuscarClientePorId(int id) => _clientes.FirstOrDefault(c => c.Id == id);

    public List<Cliente> BuscarClientesPorNombre(string texto)
    {
        return _clientes.Where(c => c.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool EliminarCliente(int id)
    {
        var cliente = BuscarClientePorId(id);
        if (cliente == null) return false;
        _mascotas.RemoveAll(m => m.ClienteId == id);
        _clientes.Remove(cliente);
        return true;
    }

    public Mascota AgregarMascota(string nombre, string especie, string raza, int edad, int clienteId)
    {
        if (BuscarClientePorId(clienteId) == null)
            throw new InvalidOperationException("El cliente no existe.");

        var mascota = new Mascota
        {
            Id = _nextMascotaId++,
            Nombre = nombre,
            Especie = especie,
            Raza = raza,
            Edad = edad,
            ClienteId = clienteId
        };
        _mascotas.Add(mascota);
        return mascota;
    }

    public List<Mascota> ListarMascotas() => _mascotas;

    public List<Mascota> ListarMascotasDeCliente(int clienteId)
    {
        return _mascotas.Where(m => m.ClienteId == clienteId).ToList();
    }

    public Mascota? BuscarMascotaPorId(int id) => _mascotas.FirstOrDefault(m => m.Id == id);

    public bool EliminarMascota(int id)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;
        _mascotas.Remove(mascota);
        return true;
    }
}
