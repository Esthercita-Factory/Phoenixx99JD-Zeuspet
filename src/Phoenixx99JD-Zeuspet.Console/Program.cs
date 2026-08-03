using Phoenixx99JD_Zeuspet.Console.Services;

namespace Phoenixx99JD_Zeuspet.Console;

public class Program
{
    private static readonly VeterinariaService Service = new();

    public static void Main()
    {
        while (true)
        {
            System.Console.Clear();
            System.Console.WriteLine("=== CLINICA VETERINARIA ZEUSPET ===");
            System.Console.WriteLine("1. Gestionar Clientes");
            System.Console.WriteLine("2. Gestionar Mascotas");
            System.Console.WriteLine("3. Salir");
            System.Console.Write("\nOpcion: ");

            switch (System.Console.ReadLine())
            {
                case "1": MenuClientes(); break;
                case "2": MenuMascotas(); break;
                case "3": return;
                default: System.Console.WriteLine("Opcion no valida."); Pausar(); break;
            }
        }
    }

    private static void MenuClientes()
    {
        System.Console.Clear();
        System.Console.WriteLine("=== GESTION DE CLIENTES ===");
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
            default: System.Console.WriteLine("Opcion no valida."); Pausar(); break;
        }
    }

    private static void AgregarCliente()
    {
        System.Console.Clear();
        System.Console.WriteLine("--- AGREGAR CLIENTE ---");
        System.Console.Write("Nombre: ");
        var nombre = System.Console.ReadLine() ?? "";
        System.Console.Write("Telefono: ");
        var telefono = System.Console.ReadLine() ?? "";
        System.Console.Write("Email: ");
        var email = System.Console.ReadLine() ?? "";
        System.Console.Write("Direccion: ");
        var direccion = System.Console.ReadLine() ?? "";

        var cliente = Service.AgregarCliente(nombre, telefono, email, direccion);
        System.Console.WriteLine($"\nCliente registrado con ID {cliente.Id}.");
        Pausar();
    }

    private static void ListarClientes()
    {
        System.Console.Clear();
        System.Console.WriteLine("--- LISTA DE CLIENTES ---");
        var clientes = Service.ListarClientes();
        if (clientes.Count == 0)
        {
            System.Console.WriteLine("No hay clientes registrados.");
        }
        else
        {
            foreach (var c in clientes)
            {
                System.Console.WriteLine($"  ID: {c.Id} | {c.Nombre} | Tel: {c.Telefono} | Email: {c.Email}");
            }
        }
        Pausar();
    }

    private static void BuscarCliente()
    {
        System.Console.Clear();
        System.Console.Write("Ingrese texto para buscar (nombre): ");
        var texto = System.Console.ReadLine() ?? "";
        var resultados = Service.BuscarClientesPorNombre(texto);
        if (resultados.Count == 0)
            System.Console.WriteLine("No se encontraron clientes.");
        else
            foreach (var c in resultados)
                System.Console.WriteLine($"  ID: {c.Id} | {c.Nombre} | Tel: {c.Telefono} | Email: {c.Email}");
        Pausar();
    }

    private static void EliminarCliente()
    {
        System.Console.Clear();
        System.Console.Write("ID del cliente a eliminar: ");
        if (int.TryParse(System.Console.ReadLine(), out int id))
        {
            if (Service.EliminarCliente(id))
                System.Console.WriteLine("Cliente eliminado.");
            else
                System.Console.WriteLine("Cliente no encontrado.");
        }
        else
        {
            System.Console.WriteLine("ID invalido.");
        }
        Pausar();
    }

    private static void MenuMascotas()
    {
        System.Console.Clear();
        System.Console.WriteLine("=== GESTION DE MASCOTAS ===");
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
            default: System.Console.WriteLine("Opcion no valida."); Pausar(); break;
        }
    }

    private static void AgregarMascota()
    {
        System.Console.Clear();
        System.Console.WriteLine("--- AGREGAR MASCOTA ---");
        System.Console.Write("Nombre: ");
        var nombre = System.Console.ReadLine() ?? "";
        System.Console.Write("Especie (ej: Perro, Gato): ");
        var especie = System.Console.ReadLine() ?? "";
        System.Console.Write("Raza: ");
        var raza = System.Console.ReadLine() ?? "";
        System.Console.Write("Edad (anios): ");
        int.TryParse(System.Console.ReadLine(), out int edad);
        System.Console.Write("ID del dueno: ");
        int.TryParse(System.Console.ReadLine(), out int clienteId);

        try
        {
            var mascota = Service.AgregarMascota(nombre, especie, raza, edad, clienteId);
            System.Console.WriteLine($"\nMascota registrada con ID {mascota.Id}.");
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
        System.Console.WriteLine("--- LISTA DE MASCOTAS ---");
        var mascotas = Service.ListarMascotas();
        if (mascotas.Count == 0)
        {
            System.Console.WriteLine("No hay mascotas registradas.");
        }
        else
        {
            foreach (var m in mascotas)
            {
                var dueno = Service.BuscarClientePorId(m.ClienteId);
                System.Console.WriteLine($"  ID: {m.Id} | {m.Nombre} | {m.Especie} | {m.Raza} | {m.Edad} anios | Dueno: {dueno?.Nombre ?? "N/A"}");
            }
        }
        Pausar();
    }

    private static void ListarMascotasDeCliente()
    {
        System.Console.Clear();
        System.Console.Write("ID del cliente: ");
        if (int.TryParse(System.Console.ReadLine(), out int id))
        {
            var mascotas = Service.ListarMascotasDeCliente(id);
            if (mascotas.Count == 0)
                System.Console.WriteLine("El cliente no tiene mascotas o no existe.");
            else
                foreach (var m in mascotas)
                    System.Console.WriteLine($"  ID: {m.Id} | {m.Nombre} | {m.Especie} | {m.Raza} | {m.Edad} anios");
        }
        else
        {
            System.Console.WriteLine("ID invalido.");
        }
        Pausar();
    }

    private static void EliminarMascota()
    {
        System.Console.Clear();
        System.Console.Write("ID de la mascota a eliminar: ");
        if (int.TryParse(System.Console.ReadLine(), out int id))
        {
            if (Service.EliminarMascota(id))
                System.Console.WriteLine("Mascota eliminada.");
            else
                System.Console.WriteLine("Mascota no encontrada.");
        }
        else
        {
            System.Console.WriteLine("ID invalido.");
        }
        Pausar();
    }

    private static void Pausar()
    {
        System.Console.WriteLine("\nPresione una tecla para continuar...");
        System.Console.ReadKey();
    }
}
