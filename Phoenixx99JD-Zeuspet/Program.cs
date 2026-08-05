using Phoenixx99JD_Zeuspet.UI;

namespace Phoenixx99JD_Zeuspet;

public class Program
{
    public static void Main()
    {
        ConsolaUI.ObtenerService().CargarDatosEjemplo();

        while (true)
        {
            System.Console.Clear();
            ConsolaUI.DibujarEncabezado("CLINICA VETERINARIA ZEUSPET");
            System.Console.WriteLine();
            System.Console.WriteLine("  [1] Gestionar Clientes");
            System.Console.WriteLine("  [2] Gestionar Mascotas");
            System.Console.WriteLine("  [3] Salir");
            System.Console.WriteLine();
            System.Console.Write("  Opcion: ");

            switch (System.Console.ReadLine())
            {
                case "1": ConsolaUI.MenuClientes(); break;
                case "2": ConsolaUI.MenuMascotas(); break;
                case "3": return;
                default:
                    System.Console.WriteLine("\nOpcion no valida.");
                    ConsolaUI.Pausar();
                    break;
            }
        }
    }
}
