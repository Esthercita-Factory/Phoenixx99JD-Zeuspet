using Phoenixx99JD_Zeuspet.Models;
using Phoenixx99JD_Zeuspet.Services;

namespace Phoenixx99JD_Zeuspet.UI;

public static class ConsolaUI
{
    private static readonly VeterinariaService Service = new();

    public static VeterinariaService ObtenerService() => Service;

    // ============================================
    // SECCION 1: ESTILOS VISUALES BASICOS
    // ============================================

    public static void DibujarEncabezado(string titulo)
    {
        System.Console.WriteLine($"\n=== {titulo} ===\n");
    }

    public static void DibujarTablaClientes(List<Cliente> clientes)
    {
        System.Console.WriteLine("ID         | Nombre             | Edad | Telefono     | Email");
        System.Console.WriteLine(new string('-', 76));
        foreach (var c in clientes)
            System.Console.WriteLine($"{c.Id,-10} | {Truncar(c.Nombre, 18),-18} | {c.Edad,-4} | {Truncar(c.Telefono, 12),-12} | {c.Email}");
    }

    public static void DibujarTablaMascotas(List<Mascota> mascotas, bool mostrarDueno)
    {
        if (mostrarDueno)
        {
            System.Console.WriteLine("ID         | Nombre     | Especie  | Raza             | Edad | Dueno");
            System.Console.WriteLine(new string('-', 80));
            foreach (var m in mascotas)
            {
                var dueno = Service.BuscarClientePorId(m.ClienteId);
                System.Console.WriteLine($"{m.Id,-10} | {Truncar(m.Nombre, 10),-10} | {Truncar(m.Especie, 8),-8} | {Truncar(m.Raza, 16),-16} | {m.Edad,-4} | {dueno?.Nombre ?? "N/A"}");
            }
        }
        else
        {
            System.Console.WriteLine("ID         | Nombre     | Especie  | Raza             | Edad");
            System.Console.WriteLine(new string('-', 60));
            foreach (var m in mascotas)
                System.Console.WriteLine($"{m.Id,-10} | {Truncar(m.Nombre, 10),-10} | {Truncar(m.Especie, 8),-8} | {Truncar(m.Raza, 16),-16} | {m.Edad}");
        }
    }

    public static string Truncar(string texto, int max)
    {
        if (string.IsNullOrEmpty(texto)) return "";
        return texto.Length <= max ? texto : texto[..(max - 1)] + "~";
    }

    public static void Pausar()
    {
        System.Console.WriteLine("\nPresione una tecla para continuar...");
        System.Console.ReadKey();
    }

    // ============================================
    // SECCION 2: MENU DE CLIENTES
    // ============================================

    public static void MenuClientes()
    {
        System.Console.Clear();
        DibujarEncabezado("GESTION DE CLIENTES");
        System.Console.WriteLine("1. Agregar cliente");
        System.Console.WriteLine("2. Listar clientes");
        System.Console.WriteLine("3. Buscar cliente");
        System.Console.WriteLine("4. Modificar cliente");
        System.Console.WriteLine("5. Eliminar cliente");
        System.Console.WriteLine("6. Volver");
        System.Console.Write("\nOpcion: ");

        switch (System.Console.ReadLine())
        {
            case "1": AgregarCliente(); break;
            case "2": ListarClientes(); break;
            case "3": BuscarCliente(); break;
            case "4": ModificarCliente(); break;
            case "5": EliminarCliente(); break;
            case "6": break;
            default:
                System.Console.WriteLine("\nOpcion no valida.");
                Pausar();
                break;
        }
    }

    private static void AgregarCliente()
    {
        System.Console.Clear();
        DibujarEncabezado("AGREGAR CLIENTE");
        System.Console.Write("Nombre: ");
        var nombre = System.Console.ReadLine() ?? "";
        System.Console.Write("Edad: ");
        var edadTexto = System.Console.ReadLine();
        int edad;
        while (!int.TryParse(edadTexto, out edad) || edad < 0)
        {
            System.Console.Write("Edad invalida. Ingrese un numero entero no negativo: ");
            edadTexto = System.Console.ReadLine();
        }
        System.Console.Write("Telefono: ");
        var telefono = System.Console.ReadLine() ?? "";
        System.Console.Write("Email: ");
        var email = System.Console.ReadLine() ?? "";
        System.Console.Write("Direccion: ");
        var direccion = System.Console.ReadLine() ?? "";

        var cliente = Service.AgregarCliente(nombre, telefono, email, direccion, edad);
        System.Console.WriteLine($"\nCliente registrado. ID: {cliente.Id}");
        Pausar();
    }

    private static void ListarClientes()
    {
        System.Console.Clear();
        DibujarEncabezado("LISTA DE CLIENTES");

        var clientes = Service.ListarClientes();
        if (clientes.Count == 0)
            System.Console.WriteLine("No hay clientes registrados.");
        else
            DibujarTablaClientes(clientes);

        Pausar();
    }

    private static void BuscarCliente()
    {
        System.Console.Clear();
        DibujarEncabezado("BUSCAR CLIENTE");
        System.Console.Write("Ingrese texto para buscar (nombre): ");
        var texto = System.Console.ReadLine() ?? "";
        var resultados = Service.BuscarClientesPorNombre(texto);

        System.Console.WriteLine();
        if (resultados.Count == 0)
            System.Console.WriteLine("No se encontraron clientes.");
        else
        {
            System.Console.WriteLine($"Se encontraron {resultados.Count} resultado(s):");
            DibujarTablaClientes(resultados);
        }
        Pausar();
    }

    private static void ModificarCliente()
    {
        System.Console.Clear();
        DibujarEncabezado("MODIFICAR CLIENTE");
        System.Console.Write("ID del cliente a modificar: ");
        var id = System.Console.ReadLine() ?? "";

        var cliente = Service.BuscarClientePorId(id);
        if (cliente == null)
        {
            System.Console.WriteLine("\nCliente no encontrado.");
            Pausar();
            return;
        }

        System.Console.WriteLine($"\nDatos actuales: {cliente.Nombre} | {cliente.Edad} anios | {cliente.Telefono} | {cliente.Email} | {cliente.Direccion}");
        System.Console.Write("\nNuevo nombre (Enter para mantener): ");
        var nombre = System.Console.ReadLine();
        System.Console.Write($"Nueva edad (Enter para mantener {cliente.Edad}): ");
        var edadTexto = System.Console.ReadLine();
        int? edadNueva = null;
        if (!string.IsNullOrWhiteSpace(edadTexto) && int.TryParse(edadTexto, out int edadParseada) && edadParseada >= 0)
            edadNueva = edadParseada;
        System.Console.Write("Nuevo telefono (Enter para mantener): ");
        var telefono = System.Console.ReadLine();
        System.Console.Write("Nuevo email (Enter para mantener): ");
        var email = System.Console.ReadLine();
        System.Console.Write("Nueva direccion (Enter para mantener): ");
        var direccion = System.Console.ReadLine();

        if (Service.ModificarCliente(id,
                string.IsNullOrWhiteSpace(nombre) ? cliente.Nombre : nombre,
                string.IsNullOrWhiteSpace(telefono) ? cliente.Telefono : telefono,
                string.IsNullOrWhiteSpace(email) ? cliente.Email : email,
                string.IsNullOrWhiteSpace(direccion) ? cliente.Direccion : direccion,
                edadNueva))
            System.Console.WriteLine("\nCliente modificado correctamente.");
        else
            System.Console.WriteLine("\nNo se pudo modificar el cliente.");
        Pausar();
    }

    private static void EliminarCliente()
    {
        System.Console.Clear();
        DibujarEncabezado("ELIMINAR CLIENTE");
        System.Console.Write("ID del cliente a eliminar: ");
        var id = System.Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(id))
            System.Console.WriteLine("\nID invalido.");
        else if (Service.EliminarCliente(id))
            System.Console.WriteLine("\nCliente eliminado.");
        else
            System.Console.WriteLine("\nCliente no encontrado.");

        Pausar();
    }

    // ============================================
    // SECCION 3: MENU DE MASCOTAS
    // ============================================

    public static void MenuMascotas()
    {
        System.Console.Clear();
        DibujarEncabezado("GESTION DE MASCOTAS");
        System.Console.WriteLine("1. Agregar mascota");
        System.Console.WriteLine("2. Listar todas las mascotas");
        System.Console.WriteLine("3. Listar mascotas de un cliente");
        System.Console.WriteLine("4. Modificar mascota");
        System.Console.WriteLine("5. Eliminar mascota");
        System.Console.WriteLine("6. Volver");
        System.Console.Write("\nOpcion: ");

        switch (System.Console.ReadLine())
        {
            case "1": AgregarMascota(); break;
            case "2": ListarMascotas(); break;
            case "3": ListarMascotasDeCliente(); break;
            case "4": ModificarMascota(); break;
            case "5": EliminarMascota(); break;
            case "6": break;
            default:
                System.Console.WriteLine("\nOpcion no valida.");
                Pausar();
                break;
        }
    }

    private static void AgregarMascota()
    {
        System.Console.Clear();
        DibujarEncabezado("AGREGAR MASCOTA");
        System.Console.Write("Nombre: ");
        var nombre = System.Console.ReadLine() ?? "";
        System.Console.Write("Especie: ");
        var especie = System.Console.ReadLine() ?? "";
        System.Console.Write("Raza: ");
        var raza = System.Console.ReadLine() ?? "";
        System.Console.Write("Edad (anios): ");
        int.TryParse(System.Console.ReadLine(), out int edad);
        System.Console.Write("ID del dueno: ");
        var clienteId = System.Console.ReadLine() ?? "";

        try
        {
            var mascota = Service.AgregarMascota(nombre, especie, raza, edad, clienteId);
            System.Console.WriteLine($"\nMascota registrada. ID: {mascota.Id}");
        }
        catch (InvalidOperationException ex)
        {
            System.Console.WriteLine($"\nError: {ex.Message}");
        }
        Pausar();
    }

    private static void ListarMascotas()
    {
        System.Console.Clear();
        DibujarEncabezado("LISTA DE MASCOTAS");

        var mascotas = Service.ListarMascotas();
        if (mascotas.Count == 0)
            System.Console.WriteLine("No hay mascotas registradas.");
        else
            DibujarTablaMascotas(mascotas, mostrarDueno: true);

        Pausar();
    }

    private static void ListarMascotasDeCliente()
    {
        System.Console.Clear();
        DibujarEncabezado("MASCOTAS POR CLIENTE");
        System.Console.Write("ID del cliente: ");
        var id = System.Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(id))
        {
            System.Console.WriteLine("\nID invalido.");
        }
        else
        {
            var cliente = Service.BuscarClientePorId(id);
            if (cliente == null)
            {
                System.Console.WriteLine("\nCliente no encontrado.");
            }
            else
            {
                var mascotas = Service.ListarMascotasDeCliente(id);
                System.Console.WriteLine($"\nMascotas de {cliente.Nombre}:");
                if (mascotas.Count == 0)
                    System.Console.WriteLine("Este cliente no tiene mascotas.");
                else
                    DibujarTablaMascotas(mascotas, mostrarDueno: false);
            }
        }
        Pausar();
    }

    private static void ModificarMascota()
    {
        System.Console.Clear();
        DibujarEncabezado("MODIFICAR MASCOTA");
        System.Console.Write("ID de la mascota a modificar: ");
        var id = System.Console.ReadLine() ?? "";

        var mascota = Service.BuscarMascotaPorId(id);
        if (mascota == null)
        {
            System.Console.WriteLine("\nMascota no encontrada.");
            Pausar();
            return;
        }

        System.Console.WriteLine($"\nDatos actuales: {mascota.Nombre} | {mascota.Especie} | {mascota.Raza} | {mascota.Edad} anios");
        System.Console.Write("\nNuevo nombre (Enter para mantener): ");
        var nombre = System.Console.ReadLine();
        System.Console.Write("Nueva especie (Enter para mantener): ");
        var especie = System.Console.ReadLine();
        System.Console.Write("Nueva raza (Enter para mantener): ");
        var raza = System.Console.ReadLine();
        System.Console.Write($"Nueva edad (Enter para mantener {mascota.Edad}): ");
        var edadTexto = System.Console.ReadLine();
        int.TryParse(edadTexto, out int edad);

        if (Service.ModificarMascota(id,
                string.IsNullOrWhiteSpace(nombre) ? mascota.Nombre : nombre,
                string.IsNullOrWhiteSpace(especie) ? mascota.Especie : especie,
                string.IsNullOrWhiteSpace(raza) ? mascota.Raza : raza,
                string.IsNullOrWhiteSpace(edadTexto) ? mascota.Edad : edad))
            System.Console.WriteLine("\nMascota modificada correctamente.");
        else
            System.Console.WriteLine("\nNo se pudo modificar la mascota.");
        Pausar();
    }

    private static void EliminarMascota()
    {
        System.Console.Clear();
        DibujarEncabezado("ELIMINAR MASCOTA");
        System.Console.Write("ID de la mascota a eliminar: ");
        var id = System.Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(id))
            System.Console.WriteLine("\nID invalido.");
        else if (Service.EliminarMascota(id))
            System.Console.WriteLine("\nMascota eliminada.");
        else
            System.Console.WriteLine("\nMascota no encontrada.");

        Pausar();
    }

    // ============================================
    // SECCION 4: REPORTES ASINCRONOS (async/await)
    // ============================================

    public static async Task MenuReportesAsync()
    {
        System.Console.Clear();
        DibujarEncabezado("REPORTES (PROGRAMACION ASINCRONA)");
        System.Console.WriteLine("1. Generar reporte en paralelo (Task.WhenAll)");
        System.Console.WriteLine("2. Diagnostico mas rapido (Task.WhenAny)");
        System.Console.WriteLine("3. Guardar reporte en archivo (I/O async)");
        System.Console.WriteLine("4. Volver");
        System.Console.Write("\nOpcion: ");

        switch (System.Console.ReadLine())
        {
            case "1": await GenerarReporteAsync(); break;
            case "2": await EjecutarDiagnosticoRapidoAsync(); break;
            case "3": await GuardarReporteAsync(); break;
            case "4": break;
            default:
                System.Console.WriteLine("\nOpcion no valida.");
                Pausar();
                break;
        }
    }

    private static async Task GenerarReporteAsync()
    {
        System.Console.Clear();
        DibujarEncabezado("GENERAR REPORTE EN PARALELO (Task.WhenAll)");
        System.Console.WriteLine("Ejecutando procesos en paralelo, espere...\n");

        IReadOnlyList<string> secciones = await Service.GenerarReporteParaleloAsync();

        foreach (string seccion in secciones)
            System.Console.WriteLine("  " + seccion);

        System.Console.WriteLine("\nTodas las secciones del reporte estan listas (Task.WhenAll).");
        Pausar();
    }

    private static async Task EjecutarDiagnosticoRapidoAsync()
    {
        System.Console.Clear();
        DibujarEncabezado("DIAGNOSTICO MAS RAPIDO (Task.WhenAny)");
        System.Console.WriteLine("Ejecutando diagnosticos en paralelo, espere...\n");

        var (etiqueta, resultado) = await Service.EjecutarDiagnosticoRapidoAsync();

        System.Console.WriteLine($"  {etiqueta}: {resultado}");
        System.Console.WriteLine("\nSolo se espero por el proceso que termino primero (Task.WhenAny).");
        Pausar();
    }

    private static async Task GuardarReporteAsync()
    {
        System.Console.Clear();
        DibujarEncabezado("GUARDAR REPORTE EN ARCHIVO (I/O async)");
        System.Console.Write("Ruta del archivo (Enter para 'reporte-clinica.txt'): ");
        var ruta = System.Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ruta))
            ruta = "reporte-clinica.txt";

        System.Console.WriteLine("\nGenerando y guardando el reporte, espere...\n");
        string archivo = await Service.GuardarReporteAsync(ruta);

        System.Console.WriteLine($"Reporte guardado en: {archivo}");
        System.Console.WriteLine("La escritura se realizo de forma asincrona sin bloquear el hilo principal.");
        Pausar();
    }
}
