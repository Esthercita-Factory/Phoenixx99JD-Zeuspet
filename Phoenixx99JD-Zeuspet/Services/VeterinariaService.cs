using Phoenixx99JD_Zeuspet.Models;

namespace Phoenixx99JD_Zeuspet.Services;

public class VeterinariaService
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Mascota> _mascotas = [];

    public Cliente AgregarCliente(string nombre, string telefono, string email, string direccion)
    {
        var cliente = new Cliente(nombre, telefono, email, direccion);
        _clientes.Add(cliente);
        return cliente;
    }

    public List<Cliente> ListarClientes() => _clientes;

    public Cliente? BuscarClientePorId(string id) => _clientes.FirstOrDefault(c => c.Id == id);

    public List<Cliente> BuscarClientesPorNombre(string texto)
    {
        return _clientes.Where(c => c.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool EliminarCliente(string id)
    {
        var cliente = BuscarClientePorId(id);
        if (cliente == null) return false;
        _mascotas.RemoveAll(m => m.ClienteId == id);
        _clientes.Remove(cliente);
        return true;
    }

    public Mascota AgregarMascota(string nombre, string especie, string raza, int edad, string clienteId)
    {
        if (BuscarClientePorId(clienteId) == null)
            throw new InvalidOperationException("El cliente no existe.");

        var mascota = new Mascota(nombre, especie, raza, edad, clienteId);
        _mascotas.Add(mascota);
        return mascota;
    }

    public List<Mascota> ListarMascotas() => _mascotas;

    public List<Mascota> ListarMascotasDeCliente(string clienteId)
    {
        return _mascotas.Where(m => m.ClienteId == clienteId).ToList();
    }

    public Mascota? BuscarMascotaPorId(string id) => _mascotas.FirstOrDefault(m => m.Id == id);

    public bool EliminarMascota(string id)
    {
        var mascota = BuscarMascotaPorId(id);
        if (mascota == null) return false;
        _mascotas.Remove(mascota);
        return true;
    }

    public void CargarDatosEjemplo()
    {
        var c1 = AgregarCliente("Carlos Mendoza", "3001234567", "carlos@email.com", "Calle 45 #12-30");
        var c2 = AgregarCliente("Ana Lucia Torres", "3109876543", "ana.torres@email.com", "Carrera 15 #80-22");
        var c3 = AgregarCliente("Roberto Jimenez", "3205551234", "roberto.j@email.com", "Av. Siempre Viva #34");
        var c4 = AgregarCliente("Maria Fernanda Ruiz", "3157778899", "mfr@email.com", "Calle 100 #20-15");
        var c5 = AgregarCliente("Pedro Gomez", "3014445566", "pedro.gomez@email.com", "Transversal 8 #45-60");

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
