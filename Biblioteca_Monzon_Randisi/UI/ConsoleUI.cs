using Biblioteca_Monzon_Randisi.Data;
using Biblioteca_Monzon_Randisi.Models;
using Biblioteca_Monzon_Randisi.Services;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Monzon_Randisi.UI;

public class ConsoleUI
{
    private readonly InterfazBiblioteca _service;
    private readonly BibliotecaContext _context;

    public ConsoleUI(InterfazBiblioteca service, BibliotecaContext context)
    {
        _service = service;
        _context = context;
    }

    public async Task MostrarMenuPrincipalAsync()
    {
        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("BIBLIOTECA");
            Console.WriteLine("1. Buscar libros");
            Console.WriteLine("2. Realizar préstamo");
            Console.WriteLine("3. Realizar devolución");
            Console.WriteLine("4. Reservar libro");
            Console.WriteLine("5. Ver información de socio");
            Console.WriteLine("6. Ver libros disponibles");
            Console.WriteLine("7. Ver libros más prestados");
            Console.WriteLine("8. Salir");
            Console.Write("\nSeleccione una opción: ");

            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await BuscarLibrosAsync();
                    break;
                case "2":
                    await RealizarPrestamoAsync();
                    break;
                case "3":
                    await RealizarDevolucionAsync();
                    break;
                case "4":
                    await RealizarReservaAsync();
                    break;
                case "5":
                    await VerInfoSocioAsync();
                    break;
                case "6":
                    await VerLibrosDisponiblesAsync();
                    break;
                case "7":
                    MostrarLibrosMasPrestados();
                    break;
                case "8":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar:");
                Console.ReadKey();
            }
        }
    }

    private async Task BuscarLibrosAsync()
    {
        Console.Clear();
        Console.WriteLine("BUSCAR LIBROS");
        Console.Write("Ingrese título o autor: ");
        var texto = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(texto))
        {
            Console.WriteLine("Texto de búsqueda inválido");
            return;
        }

        var libros = await _service.BuscarLibrosAsync(texto);

        if (!libros.Any())
        {
            Console.WriteLine("No se encontraron libros");
            return;
        }

        Console.WriteLine("\nResultados:");
        foreach (var libro in libros)
        {
            var copias = await _service.ObtenerCopiasDisponiblesAsync(libro.ISBN);
            Console.WriteLine($"{libro.Titulo} - {libro.Autor} (ISBN: {libro.ISBN})");
            Console.WriteLine($"Copias disponibles: {copias}");
            Console.WriteLine();
        }
    }

    private async Task RealizarPrestamoAsync()
    {
        Console.Clear();
        Console.WriteLine("REALIZAR PRÉSTAMO");

        Console.Write("Ingrese número de socio: ");
        if (!int.TryParse(Console.ReadLine(), out int nroSocio))
        {
            Console.WriteLine("Número de socio inválido");
            return;
        }

        Console.Write("Ingrese ISBN del libro: ");
        var isbn = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("ISBN inválido");
            return;
        }

        try
        {
            var resultado = await _service.PrestarLibroAsync(nroSocio, isbn);
            if (resultado)
            {
                Console.WriteLine("Préstamo registrado exitosamente");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task RealizarDevolucionAsync()
    {
        Console.Clear();
        Console.WriteLine("REALIZAR DEVOLUCIÓN");

        Console.Write("Ingrese número de socio: ");
        if (!int.TryParse(Console.ReadLine(), out int nroSocio))
        {
            Console.WriteLine("Número de socio inválido");
            return;
        }

        var prestamosActivos = await _service.ObtenerPrestamosActivosAsync(nroSocio);
        if (!prestamosActivos.Any())
        {
            Console.WriteLine("No hay préstamos activos para este socio");
            return;
        }

        Console.WriteLine("\nPréstamos activos:");
        foreach (var p in prestamosActivos)
        {
            Console.WriteLine($"{p.Libro?.Titulo} (ISBN: {p.LibroId}) - Vence: {p.FechaVencimiento}");
        }

        Console.Write("\nIngrese ISBN del libro a devolver: ");
        var isbn = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("ISBN inválido");
            return;
        }

        try
        {
            var resultado = await _service.DevolverLibroAsync(nroSocio, isbn);
            if (resultado)
            {
                Console.WriteLine("Devolución registrada exitosamente");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task RealizarReservaAsync()
    {
        Console.Clear();
        Console.WriteLine("REALIZAR RESERVA");

        Console.Write("Ingrese número de socio: ");
        if (!int.TryParse(Console.ReadLine(), out int nroSocio))
        {
            Console.WriteLine("Número de socio inválido");
            return;
        }

        Console.Write("Ingrese ISBN del libro: ");
        var isbn = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("ISBN inválido");
            return;
        }

        try
        {
            var resultado = await _service.ReservarLibroAsync(nroSocio, isbn);
            if (resultado)
            {
                Console.WriteLine("Reserva registrada exitosamente");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task VerInfoSocioAsync()
    {
        Console.Clear();
        Console.WriteLine("INFORMACIÓN DEL SOCIO");

        Console.Write("Ingrese número de socio: ");
        if (!int.TryParse(Console.ReadLine(), out int nroSocio))
        {
            Console.WriteLine("Número de socio inválido");
            return;
        }

        var socio = await _service.ObtenerSocioAsync(nroSocio);
        if (socio == null)
        {
            Console.WriteLine("Socio no encontrado");
            return;
        }

        Console.WriteLine($"\nSocio: {socio.Nombre} {socio.Apellido}");
        Console.WriteLine($"Email: {socio.Email}");
        Console.WriteLine($"Tipo: {socio.TipoSocioNavigation?.Descripcion}");
        Console.WriteLine($"Estado: {(socio.Activo == 1 ? "Activo" : "Inactivo")}");

        var prestamosActivos = await _service.ObtenerPrestamosActivosAsync(nroSocio);
        Console.WriteLine($"\nPréstamos activos: {prestamosActivos.Count}");
        foreach (var p in prestamosActivos)
        {
            Console.WriteLine($"{p.Libro?.Titulo} (Vence: {p.FechaVencimiento})");
        }

        var multaPendiente = await _service.CalcularMultaPendienteAsync(nroSocio);
        Console.WriteLine($"\nMulta pendiente: ${multaPendiente}");

        var historial = await _context.Prestamos
            .Where(p => p.SocioId == nroSocio && p.Estado == 2)
            .Include(p => p.Libro)
            .ToListAsync();

        if (historial.Any())
        {
            Console.WriteLine($"\nHistorial de devoluciones: {historial.Count}");
            foreach (var p in historial)
            {
                Console.WriteLine($"   - {p.Libro?.Titulo} (Devuelto: {p.FechaDevolucion})");
            }
        }
    }

    private async Task VerLibrosDisponiblesAsync()
    {
        Console.Clear();
        Console.WriteLine("LIBROS DISPONIBLES");

        var libros = await _service.ObtenerLibrosDisponiblesAsync();

        if (!libros.Any())
        {
            Console.WriteLine("No hay libros disponibles en este momento");
            return;
        }

        foreach (var libro in libros)
        {
            var copias = await _service.ObtenerCopiasDisponiblesAsync(libro.ISBN);
            Console.WriteLine($"{libro.Titulo} - {libro.Autor}");
            Console.WriteLine($"   ISBN: {libro.ISBN}");
            Console.WriteLine($"   Copias disponibles: {copias}");
            Console.WriteLine();
        }
    }
    public static void MostrarLibrosMasPrestados()
    {
        using var contexto = new BibliotecaContext();

        var top5 = contexto.Libros
            .Select(l => new
            {
                l.ISBN,
                l.Titulo,
                l.Autor,
                CantidadPrestamos = l.Prestamos.Count()
            })
            .OrderByDescending(x => x.CantidadPrestamos)
            .Take(5)
            .ToList();

        Console.WriteLine();
        Console.WriteLine("=== TOP 5 LIBROS MÁS PRESTADOS ===");

        if (top5.Count == 0)
        {
            Console.WriteLine("No hay préstamos registrados.");
            return;
        }

        int puesto = 1;
        foreach (var libro in top5)
        {
            Console.WriteLine($"{puesto}. [{libro.ISBN}] {libro.Titulo} - {libro.Autor} " +
                               $"=> {libro.CantidadPrestamos} préstamo(s)");
            puesto++;
        }
    }
}