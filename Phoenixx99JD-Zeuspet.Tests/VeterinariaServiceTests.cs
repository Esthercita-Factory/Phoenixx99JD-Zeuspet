using Phoenixx99JD_Zeuspet.Services;

namespace Phoenixx99JD_Zeuspet.Tests;

public class VeterinariaServiceTests
{
    private VeterinariaService _service = null!;

    [SetUp]
    public void Setup()
    {
        _service = new VeterinariaService();
    }

    [Test]
    public void AgregarCliente_DeberiaCrearClienteConIdAlfanumerico()
    {
        var cliente1 = _service.AgregarCliente("Juan", "123456", "juan@email.com", "Calle 1");
        var cliente2 = _service.AgregarCliente("Maria", "654321", "maria@email.com", "Calle 2");

        Assert.That(cliente1.Id, Is.Not.Null);
        Assert.That(cliente1.Id.Length, Is.EqualTo(8));
        Assert.That(cliente2.Id, Is.Not.EqualTo(cliente1.Id));
        Assert.That(cliente1.Nombre, Is.EqualTo("Juan"));
        Assert.That(cliente2.Nombre, Is.EqualTo("Maria"));
    }

    [Test]
    public void ListarClientes_DeberiaRetornarTodosLosClientes()
    {
        _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarCliente("Maria", "456", "maria@email.com", "Calle 2");

        var clientes = _service.ListarClientes();

        Assert.That(clientes.Count, Is.EqualTo(2));
    }

    [Test]
    public void BuscarClientePorId_DeberiaRetornarClienteExistente()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");

        var encontrado = _service.BuscarClientePorId(cliente.Id);

        Assert.That(encontrado, Is.Not.Null);
        Assert.That(encontrado!.Nombre, Is.EqualTo("Juan"));
    }

    [Test]
    public void BuscarClientePorId_DeberiaRetornarNullSiNoExiste()
    {
        var encontrado = _service.BuscarClientePorId("NOEXIST");

        Assert.That(encontrado, Is.Null);
    }

    [Test]
    public void BuscarClientesPorNombre_DeberiaRetornarCoincidencias()
    {
        _service.AgregarCliente("Juan Perez", "123", "juan@email.com", "Calle 1");
        _service.AgregarCliente("Maria Lopez", "456", "maria@email.com", "Calle 2");
        _service.AgregarCliente("Juanita Gomez", "789", "juanita@email.com", "Calle 3");

        var resultados = _service.BuscarClientesPorNombre("Juan");

        Assert.That(resultados.Count, Is.EqualTo(2));
    }

    [Test]
    public void EliminarCliente_DeberiaEliminarClienteYMascotas()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);

        var resultado = _service.EliminarCliente(cliente.Id);

        Assert.That(resultado, Is.True);
        Assert.That(_service.ListarClientes().Count, Is.EqualTo(0));
        Assert.That(_service.ListarMascotas().Count, Is.EqualTo(0));
    }

    [Test]
    public void EliminarCliente_DeberiaRetornarFalseSiNoExiste()
    {
        var resultado = _service.EliminarCliente("NOEXIST");

        Assert.That(resultado, Is.False);
    }

    [Test]
    public void AgregarMascota_DeberiaCrearMascotaConIdAlfanumerico()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota1 = _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        var mascota2 = _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente.Id);

        Assert.That(mascota1.Id, Is.Not.Null);
        Assert.That(mascota1.Id.Length, Is.EqualTo(8));
        Assert.That(mascota2.Id, Is.Not.EqualTo(mascota1.Id));
        Assert.That(mascota1.Nombre, Is.EqualTo("Firulais"));
        Assert.That(mascota2.Nombre, Is.EqualTo("Michi"));
    }

    [Test]
    public void AgregarMascota_DeberiaLanzarExcepcionSiClienteNoExiste()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, "NOEXIST"));

        Assert.That(ex!.Message, Is.EqualTo("El cliente no existe."));
    }

    [Test]
    public void ListarMascotas_DeberiaRetornarTodasLasMascotas()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente.Id);

        var mascotas = _service.ListarMascotas();

        Assert.That(mascotas.Count, Is.EqualTo(2));
    }

    [Test]
    public void ListarMascotasDeCliente_DeberiaRetornarSoloMascotasDelCliente()
    {
        var cliente1 = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var cliente2 = _service.AgregarCliente("Maria", "456", "maria@email.com", "Calle 2");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente1.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente1.Id);
        _service.AgregarMascota("Rex", "Perro", "Pastor", 5, cliente2.Id);

        var mascotasCliente1 = _service.ListarMascotasDeCliente(cliente1.Id);
        var mascotasCliente2 = _service.ListarMascotasDeCliente(cliente2.Id);

        Assert.That(mascotasCliente1.Count, Is.EqualTo(2));
        Assert.That(mascotasCliente2.Count, Is.EqualTo(1));
    }

    [Test]
    public void BuscarMascotaPorId_DeberiaRetornarMascotaExistente()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota = _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);

        var encontrada = _service.BuscarMascotaPorId(mascota.Id);

        Assert.That(encontrada, Is.Not.Null);
        Assert.That(encontrada!.Nombre, Is.EqualTo("Firulais"));
    }

    [Test]
    public void BuscarMascotaPorId_DeberiaRetornarNullSiNoExiste()
    {
        var encontrada = _service.BuscarMascotaPorId("NOEXIST");

        Assert.That(encontrada, Is.Null);
    }

    [Test]
    public void EliminarMascota_DeberiaEliminarMascota()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota = _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);

        var resultado = _service.EliminarMascota(mascota.Id);

        Assert.That(resultado, Is.True);
        Assert.That(_service.ListarMascotas().Count, Is.EqualTo(0));
    }

    [Test]
    public void EliminarMascota_DeberiaRetornarFalseSiNoExiste()
    {
        var resultado = _service.EliminarMascota("NOEXIST");

        Assert.That(resultado, Is.False);
    }
}
