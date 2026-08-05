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
        System.Console.WriteLine("ID         | Nombre             | Telefono     | Email");
        System.Console.WriteLine(new string('-', 70));
        foreach (var c in clientes)
            System.Console.WriteLine($"{c.Id,-10} | {Truncar(c.Nombre, 18),-18} | {Truncar(c.Telefono, 12),-12} | {c.Email}");
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
        System.Console.WriteLine("4. Eliminar cliente");
        System.Console.WriteLine("5. Volver");
        System.Console.Write("\nOpcion: ");

        switch (System.Console.ReadLine())
        {
            case "1": AgregarCliente(); break;
            case "2": ListarClientes(); break;
            case "3": BuscarCliente(); break;
            case "4": EliminarCliente(); break;
            case "5": break;
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
        System.Console.Write("Telefono: ");
        var telefono = System.Console.ReadLine() ?? "";
        System.Console.Write("Email: ");
        var email = System.Console.ReadLine() ?? "";
        System.Console.Write("Direccion: ");
        var direccion = System.Console.ReadLine() ?? "";

        var cliente = Service.AgregarCliente(nombre, telefono, email, direccion);
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
        System.Console.WriteLine("4. Eliminar mascota");
        System.Console.WriteLine("5. Volver");
        System.Console.Write("\nOpcion: ");

        switch (System.Console.ReadLine())
        {
            case "1": AgregarMascota(); break;
            case "2": ListarMascotas(); break;
            case "3": ListarMascotasDeCliente(); break;
            case "4": EliminarMascota(); break;
            case "5": break;
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
}
