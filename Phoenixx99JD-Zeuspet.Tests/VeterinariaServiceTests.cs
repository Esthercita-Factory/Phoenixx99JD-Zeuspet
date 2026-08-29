using Phoenixx99JD_Zeuspet.Models;
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

    [Test]
    public void ModificarCliente_DeberiaActualizarLosDatos()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");

        var resultado = _service.ModificarCliente(cliente.Id, "Juan Perez", "999", "juan2@email.com", "Calle 2");

        Assert.That(resultado, Is.True);
        Assert.That(cliente.Nombre, Is.EqualTo("Juan Perez"));
        Assert.That(cliente.Telefono, Is.EqualTo("999"));
        Assert.That(cliente.Email, Is.EqualTo("juan2@email.com"));
        Assert.That(cliente.Direccion, Is.EqualTo("Calle 2"));
    }

    [Test]
    public void ModificarCliente_DeberiaRetornarFalseSiNoExiste()
    {
        var resultado = _service.ModificarCliente("NOEXIST", "Juan", "123", "juan@email.com", "Calle 1");

        Assert.That(resultado, Is.False);
    }

    [Test]
    public void ModificarMascota_DeberiaActualizarLosDatos()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota = _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);

        var resultado = _service.ModificarMascota(mascota.Id, "Rex", "Perro", "Pastor", 5);

        Assert.That(resultado, Is.True);
        Assert.That(mascota.Nombre, Is.EqualTo("Rex"));
        Assert.That(mascota.Raza, Is.EqualTo("Pastor"));
        Assert.That(mascota.Edad, Is.EqualTo(5));
    }

    [Test]
    public void ObtenerClientePorIdRapido_DeberiaRetornarClienteDesdeDiccionario()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");

        var encontrado = _service.ObtenerClientePorIdRapido(cliente.Id);

        Assert.That(encontrado, Is.Not.Null);
        Assert.That(encontrado!.Nombre, Is.EqualTo("Juan"));
    }

    [Test]
    public void ObtenerClientePorIdRapido_DeberiaRetornarNullSiNoExiste()
    {
        var encontrado = _service.ObtenerClientePorIdRapido("NOEXIST");

        Assert.That(encontrado, Is.Null);
    }

    [Test]
    public void FiltrarClientesPorEspecie_DeberiaRetornarSoloClientesConEsaEspecie()
    {
        var c1 = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var c2 = _service.AgregarCliente("Maria", "456", "maria@email.com", "Calle 2");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, c1.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, c2.Id);

        var conPerros = _service.FiltrarClientesPorEspecie("Perro");

        Assert.That(conPerros.Count, Is.EqualTo(1));
        Assert.That(conPerros[0].Nombre, Is.EqualTo("Juan"));
    }

    [Test]
    public void FiltrarClientesPorEspecieQuery_DeberiaCoincidirConLaDeMetodos()
    {
        var c1 = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var c2 = _service.AgregarCliente("Maria", "456", "maria@email.com", "Calle 2");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, c1.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, c2.Id);

        var deMetodos = _service.FiltrarClientesPorEspecie("Gato");
        var deConsulta = _service.FiltrarClientesPorEspecieQuery("Gato");

        Assert.That(deConsulta.Count, Is.EqualTo(deMetodos.Count));
        Assert.That(deConsulta[0].Nombre, Is.EqualTo(deMetodos[0].Nombre));
    }

    [Test]
    public void FiltrarMascotasPorEdad_DeberiaRetornarMascotasConEdadMayorOIgual()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 6, cliente.Id);

        var adultas = _service.FiltrarMascotasPorEdad(4);

        Assert.That(adultas.Count, Is.EqualTo(1));
        Assert.That(adultas[0].Nombre, Is.EqualTo("Michi"));
    }

    [Test]
    public void ObtenerNombresClientes_DeberiaProyectarSoloLosNombres()
    {
        _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarCliente("Maria", "456", "maria@email.com", "Calle 2");

        var nombres = _service.ObtenerNombresClientes();

        Assert.That(nombres.Count, Is.EqualTo(2));
        Assert.That(nombres, Does.Contain("Juan"));
        Assert.That(nombres, Does.Contain("Maria"));
    }

    [Test]
    public void OrdenarMascotasPorEdadDescendente_DeberiaOrdenarDeMayorAMenor()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 6, cliente.Id);
        _service.AgregarMascota("Rex", "Perro", "Pastor", 1, cliente.Id);

        var ordenadas = _service.OrdenarMascotasPorEdadDescendente();

        Assert.That(ordenadas[0].Edad, Is.EqualTo(6));
        Assert.That(ordenadas[^1].Edad, Is.EqualTo(1));
    }

    [Test]
    public void ContarMascotasPorEspecie_DeberiaContarCadaEspecie()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Rex", "Perro", "Pastor", 4, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente.Id);

        var conteo = _service.ContarMascotasPorEspecie();

        Assert.That(conteo["Perro"], Is.EqualTo(2));
        Assert.That(conteo["Gato"], Is.EqualTo(1));
    }

    [Test]
    public void ObtenerMascotaMasJoven_DeberiaRetornarLaDeMenorEdad()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 6, cliente.Id);
        _service.AgregarMascota("Rex", "Perro", "Pastor", 1, cliente.Id);

        var joven = _service.ObtenerMascotaMasJoven();

        Assert.That(joven.Nombre, Is.EqualTo("Rex"));
    }

    [Test]
    public void ObtenerMascotaDeMayorEdad_DeberiaRetornarLaDeMayorEdad()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 6, cliente.Id);

        var mayor = _service.ObtenerMascotaDeMayorEdad();

        Assert.That(mayor.Nombre, Is.EqualTo("Michi"));
    }

    [Test]
    public void ExisteMascotaSinRaza_DeberiaDetectarMascotaSinRaza()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Sombra", "Gato", "", 4, cliente.Id);

        Assert.That(_service.ExisteMascotaSinRaza(), Is.True);
        Assert.That(_service.TodasLasMascotasTienenRaza(), Is.False);
    }

    [Test]
    public void ObtenerNombresClientesEnMayusculasOrdenados_DeberiaProyectarOrdenado()
    {
        _service.AgregarCliente("maria", "123", "maria@email.com", "Calle 1");
        _service.AgregarCliente("juan", "456", "juan@email.com", "Calle 2");

        var nombres = _service.ObtenerNombresClientesEnMayusculasOrdenados();

        Assert.That(nombres.Count, Is.EqualTo(2));
        Assert.That(nombres[0], Is.EqualTo("JUAN"));
        Assert.That(nombres[1], Is.EqualTo("MARIA"));
    }

    [Test]
    public void ObtenerDuenosDePerrosOrdenadosPorEdad_DeberiaProyectarNombreYTelefono()
    {
        var c1 = _service.AgregarCliente("Juan", "111", "juan@email.com", "Calle 1");
        var c2 = _service.AgregarCliente("Maria", "222", "maria@email.com", "Calle 2");
        _service.AgregarMascota("Rex", "Perro", "Pastor", 5, c1.Id);
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 2, c2.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 3, c1.Id);

        var resultados = _service.ObtenerDuenosDePerrosOrdenadosPorEdad();

        Assert.That(resultados.Count, Is.EqualTo(2));
        Assert.That(resultados[0].Dueno, Is.EqualTo("Maria"));
        Assert.That(resultados[0].Telefono, Is.EqualTo("222"));
        Assert.That(resultados[0].EdadMascota, Is.EqualTo(2));
        Assert.That(resultados[1].Dueno, Is.EqualTo("Juan"));
    }

    [Test]
    public void AnyAllCount_DeberianFuncionarSobreLaColeccion()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente.Id);

        Assert.That(_service.ExisteAlgunaMascotaDeEspecie("Perro"), Is.True);
        Assert.That(_service.ExisteAlgunaMascotaDeEspecie("Conejo"), Is.False);
        Assert.That(_service.ContarMascotasDeEspecie("Perro"), Is.EqualTo(1));
        Assert.That(_service.ObtenerPrimerPerro()?.Nombre, Is.EqualTo("Firulais"));
        Assert.That(_service.ObtenerPrimeraMascota().Nombre, Is.EqualTo("Firulais"));
    }

    // ============================================
    // UML / POO: herencia y polimorfismo
    // ============================================

    [Test]
    public void Mascota_DeberiaSerUnAnimal()
    {
        var mascota = new Mascota("Rex", "Perro", "Labrador", 3, "CLIENTE1");

        Assert.That(mascota, Is.InstanceOf<Animal>());
        Assert.That(mascota.Nombre, Is.EqualTo("Rex"));
        Assert.That(mascota.Especie, Is.EqualTo("Perro"));
        Assert.That(mascota.Edad, Is.EqualTo(3));
    }

    [Test]
    public void EmitirSonido_DeberiaDependerDeLaEspecie()
    {
        var perro = new Mascota("Rex", "Perro", "Labrador", 3, "CLIENTE1");
        var gato = new Mascota("Michi", "Gato", "Persa", 2, "CLIENTE1");
        var conejo = new Mascota("Nina", "Conejo", "Mini Lop", 1, "CLIENTE1");

        Assert.That(perro.EmitirSonido(), Is.EqualTo("Guau"));
        Assert.That(gato.EmitirSonido(), Is.EqualTo("Miau"));
        Assert.That(conejo.EmitirSonido(), Is.EqualTo("..."));
    }

    [Test]
    public void Polimorfismo_DeberiaLlamarAlMismoMetodoDesdeDiferentesTipos()
    {
        List<Animal> animales =
        [
            new Mascota("Rex", "Perro", "Labrador", 3, "CLIENTE1"),
            new Mascota("Michi", "Gato", "Persa", 2, "CLIENTE1")
        ];

        var sonidos = animales.Select(a => a.EmitirSonido()).ToList();

        Assert.That(sonidos, Does.Contain("Guau"));
        Assert.That(sonidos, Does.Contain("Miau"));
    }

    // ============================================
    // UML / POO: asociacion cliente-mascota
    // ============================================

    [Test]
    public void Cliente_DeberiaRelacionarseConSusMascotas()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota1 = _service.AgregarMascota("Firulais", "Perro", "Labrador", 3, cliente.Id);
        var mascota2 = _service.AgregarMascota("Michi", "Gato", "Persa", 2, cliente.Id);

        Assert.That(cliente.Mascotas.Count, Is.EqualTo(2));
        Assert.That(cliente.Mascotas, Does.Contain(mascota1));
        Assert.That(cliente.Mascotas, Does.Contain(mascota2));
    }

    [Test]
    public void AgregarCliente_DeberiaGuardarLaEdad()
    {
        var cliente = _service.AgregarCliente("Juan", "123", "juan@email.com", "Calle 1", 30);

        Assert.That(cliente.Edad, Is.EqualTo(30));
    }

    // ============================================
    // UML / POO: encapsulacion
    // ============================================

    [Test]
    public void Encapsulacion_EdadNegativaDeberiaRechazarse()
    {
        var cliente = new Cliente("Juan", "123", "juan@email.com", "Calle 1", -5);

        Assert.That(cliente.Edad, Is.EqualTo(0));
    }

    [Test]
    public void Encapsulacion_NombreVacioDeberiaRechazarse()
    {
        var cliente = new Cliente("  ", "123", "juan@email.com", "Calle 1");

        Assert.That(cliente.Nombre, Is.EqualTo(""));
    }

    [Test]
    public void Encapsulacion_IdSoloSeAsignaUnaVez()
    {
        var cliente = new Cliente("Juan", "123", "juan@email.com", "Calle 1");
        var idOriginal = cliente.Id;

        Assert.That(idOriginal, Is.Not.Null);
        Assert.That(cliente.Id, Is.EqualTo(idOriginal));
    }

    // ============================================
    // UML / POO: abstraccion e interfaces
    // ============================================

    [Test]
    public void ClienteY_Mascota_DeberianImplementarIRegistrable()
    {
        var cliente = new Cliente("Juan", "123", "juan@email.com", "Calle 1");
        var mascota = new Mascota("Rex", "Perro", "Labrador", 3, cliente.Id);

        Assert.That(cliente, Is.AssignableTo<IRegistrable>());
        Assert.That(mascota, Is.AssignableTo<IRegistrable>());
        Assert.That(cliente.Registrar(), Does.Contain(cliente.Nombre));
        Assert.That(mascota.Registrar(), Does.Contain(mascota.Nombre));
    }

    [Test]
    public void ServiciosVeterinarios_DeberianAtenderDiferente()
    {
        ServicioVeterinario consulta = new ConsultaGeneral();
        ServicioVeterinario vacuna = new Vacunacion();

        Assert.That(consulta.Atender(), Is.Not.EqualTo(vacuna.Atender()));
        Assert.That(consulta.Atender(), Does.Contain("Consulta"));
        Assert.That(vacuna.Atender(), Does.Contain("Vacunacion"));
    }

    // ============================================
    // PROGRAMACION ASINCRONA (async / await, Task)
    // ============================================

    [Test]
    public async Task GenerarReporteParaleloAsync_DeberiaRetornarLasTresSecciones()
    {
        IReadOnlyList<string> secciones = await _service.GenerarReporteParaleloAsync();

        Assert.That(secciones, Is.Not.Null);
        Assert.That(secciones.Count, Is.EqualTo(3));
        Assert.That(secciones[0], Does.Contain("Consultas"));
        Assert.That(secciones[1], Does.Contain("Vacunaciones"));
        Assert.That(secciones[2], Does.Contain("Seguimientos"));
    }

    [Test]
    public async Task EjecutarDiagnosticoRapidoAsync_DeberiaRetornarUnResultadoNoVacio()
    {
        var (etiqueta, resultado) = await _service.EjecutarDiagnosticoRapidoAsync();

        Assert.That(etiqueta, Is.EqualTo("Diagnostico mas rapido"));
        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado, Is.Not.Empty);
    }

    [Test]
    public async Task GuardarReporteAsync_DeberiaCrearElArchivoEnDisco()
    {
        string rutaTemp = Path.Combine(Path.GetTempPath(), $"reporte-{Guid.NewGuid():N}.txt");

        try
        {
            string archivo = await _service.GuardarReporteAsync(rutaTemp);

            Assert.That(archivo, Is.EqualTo(rutaTemp));
            Assert.That(File.Exists(rutaTemp), Is.True);
            string contenido = await File.ReadAllTextAsync(rutaTemp);
            Assert.That(contenido, Does.Contain("REPORTE SEMANAL"));
            Assert.That(contenido, Does.Contain("Clientes registrados"));
        }
        finally
        {
            if (File.Exists(rutaTemp))
                File.Delete(rutaTemp);
        }
    }

    private static async Task<string> TareaCortaAsync() =>
        await Task.Run(() => "corta");

    private static async Task<string> TareaLargaAsync() =>
        await Task.Run(async () => { await Task.Delay(300); return "larga"; });

    [Test]
    public async Task TaskWhenAny_DeberiaCompletarseConLaTareaMasRapida()
    {
        Task<string> corta = TareaCortaAsync();
        Task<string> larga = TareaLargaAsync();

        Task<string> completada = await Task.WhenAny(corta, larga);

        Assert.That(await completada, Is.EqualTo("corta"));
    }
}
