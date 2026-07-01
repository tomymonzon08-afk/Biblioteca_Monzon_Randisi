using Biblioteca_Monzon_Randisi.Data;
using Biblioteca_Monzon_Randisi.Models;
using Biblioteca_Monzon_Randisi.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            Console.WriteLine("8. Mostrar socios con multas pendientes");
            Console.WriteLine("9. Mostrar prestamos vencidos");
            Console.WriteLine("10. Mostrar disponibilidad de un libro");
            Console.WriteLine("11.Mostrar historial de un socio");
            Console.WriteLine("0. Salir");
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
                    MostrarSociosConMultasPendientes();
                    break;
                case "9":
                    MostrarPrestamosVencidos();
                    break;
                case "10":
                    Console.Write("Ingrese ISBN o título del libro: ");
                    var isbnOTitulo = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(isbnOTitulo))
                    {
                        MostrarDisponibilidadLibro(isbnOTitulo);
                    }
                    else
                    {
                        Console.WriteLine("ISBN o título inválido");
                    }
                    break;
                case "11":
                    Console.Write("Ingrese número de socio: ");
                    if (int.TryParse(Console.ReadLine(), out int nroSocio))
                    {
                        MostrarHistorialSocio(nroSocio);
                    }
                    else
                    {
                        Console.WriteLine("Número de socio inválido");
                    }
                    break;
                case "0":
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
    public static void MostrarSociosConMultasPendientes()
    {
        using var contexto = new BibliotecaContext();

        var socios = contexto.Multas
            .Include(m => m.Socio)
            .Where(m => m.Estado == "Pendiente")
            .Select(m => new
            {
                m.Socio.NroSocio,
                m.Socio.Nombre,
                m.Socio.Apellido,
                Monto = m.Valor
            })
            .OrderByDescending(x => x.Monto)
            .ToList();
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== SOCIOS CON MULTAS PENDIENTES ===");

        if (socios.Count == 0)
        {
            Console.WriteLine("No hay socios con multas pendientes.");
            return;
        }

        foreach (var s in socios)
        {
            Console.WriteLine($"Socio N° {s.NroSocio} - {s.Nombre} {s.Apellido} " +
                               $"=> Monto adeudado: ${s.Monto}");
        }

        Console.WriteLine($"Total adeudado entre todos los socios: ${socios.Sum(s => s.Monto)}");
    }

    public static void MostrarPrestamosVencidos()
    {
        using var contexto = new BibliotecaContext();

        string hoy = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var vencidos = contexto.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Libro)
            .Where(p => p.FechaDevolucion == null && p.FechaVencimiento.CompareTo(hoy) < 0)
            .OrderBy(p => p.FechaVencimiento)
            .ToList();

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== PRÉSTAMOS VENCIDOS ===");

        if (vencidos.Count == 0)
        {
            Console.WriteLine("No hay préstamos vencidos.");
            return;
        }

        foreach (var p in vencidos)
        {
            int diasVencido = (DateTime.Today - DateTime.ParseExact(p.FechaVencimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;
            Console.WriteLine($"Socio N° {p.SocioId} ({p.Socio.Nombre} {p.Socio.Apellido}) - " +
                               $"Libro: {p.Libro.Titulo} [{p.LibroId}] - " +
                               $"Vencía: {p.FechaVencimiento} - Días de atraso: {diasVencido}");
        }
    }
    public static void MostrarDisponibilidadLibro(string isbnOTitulo)
    {
        using var contexto = new BibliotecaContext();

        var libro = contexto.Libros
            .Include(l => l.Prestamos)
            .Include(l => l.Reservas)
                .ThenInclude(r => r.EstadoNavigation)
            .FirstOrDefault(l => l.ISBN == isbnOTitulo ||
                                  l.Titulo.ToLower() == isbnOTitulo.ToLower());

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== DISPONIBILIDAD DE LIBRO ===");

        if (libro == null)
        {
            Console.WriteLine("No se encontró ningún libro con ese ISBN o título.");
            return;
        }

        int prestamosActivos = libro.Prestamos.Count(p => p.FechaDevolucion == null);
        int copiasDisponibles = libro.CantCopias - prestamosActivos;
        if (copiasDisponibles < 0) copiasDisponibles = 0;

        var reservasPendientes = libro.Reservas
            .Where(r => r.EstadoNavigation.Descripcion.ToLower() == "pendiente")
            .ToList();

        Console.WriteLine($"Libro: {libro.Titulo} - {libro.Autor} [{libro.ISBN}]");
        Console.WriteLine($"Copias totales: {libro.CantCopias}");
        Console.WriteLine($"Copias prestadas actualmente: {prestamosActivos}");
        Console.WriteLine($"Copias disponibles: {copiasDisponibles}");
        Console.WriteLine(reservasPendientes.Count > 0
            ? $"Reservas pendientes: {reservasPendientes.Count}"
            : "No hay reservas pendientes.");
    }
    public static void MostrarHistorialSocio(int nroSocio)
    {
        using var contexto = new BibliotecaContext();

        var socio = contexto.Socios
            .Include(s => s.Prestamos)
                .ThenInclude(p => p.Libro)
            .Include(s => s.Prestamos)
                .ThenInclude(p => p.EstadoNavigation)
            .Include(s => s.Reservas)
                .ThenInclude(r => r.Libro)
            .Include(s => s.Reservas)
                .ThenInclude(r => r.EstadoNavigation)
            .FirstOrDefault(s => s.NroSocio == nroSocio);

        Console.WriteLine();
        Console.WriteLine("=== HISTORIAL DE SOCIO ===");

        if (socio == null)
        {
            Console.WriteLine("No se encontró ningún socio con ese número.");
            return;
        }

        Console.WriteLine($"Socio N° {socio.NroSocio} - {socio.Nombre} {socio.Apellido} ({socio.Email})");

        Console.WriteLine();
        Console.WriteLine("--- Préstamos ---");
        if (socio.Prestamos.Count == 0)
        {
            Console.WriteLine("Sin préstamos registrados.");
        }
        else
        {
            foreach (var p in socio.Prestamos.OrderByDescending(p => p.FechaPrestamo))
            {
                Console.WriteLine($"Libro: {p.Libro.Titulo} [{p.LibroId}] - " +
                                   $"Pedido: {p.FechaPrestamo} - Vence: {p.FechaVencimiento} - " +
                                   $"Devuelto: {p.FechaDevolucion ?? "No devuelto"} - " +
                                   $"Estado: {p.EstadoNavigation.Descripcion}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("--- Reservas ---");
        if (socio.Reservas.Count == 0)
        {
            Console.WriteLine("Sin reservas registradas.");
        }
        else
        {
            foreach (var r in socio.Reservas.OrderByDescending(r => r.FechaReserva))
            {
                Console.WriteLine($"Libro: {r.Libro.Titulo} [{r.LibroId}] - " +
                                   $"Reservado: {r.FechaReserva} - " +
                                   $"Estado: {r.EstadoNavigation.Descripcion}");
            }
        }
    }
}