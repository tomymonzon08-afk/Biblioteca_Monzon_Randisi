using Biblioteca_Monzon_Randisi.Data;
using Biblioteca_Monzon_Randisi.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Monzon_Randisi.Services;

public class BibliotecaService : InterfazBiblioteca
{
    private readonly BibliotecaContext _context;

    public BibliotecaService(BibliotecaContext context)
    {
        _context = context;
    }


    private string FormatearFecha(DateTime fecha)
    {
        return fecha.ToString("dd-MM-yyyy");
    }

    private DateTime ParsearFecha(string fecha)
    {
        return DateTime.ParseExact(fecha, "dd-MM-yyyy", null);
    }

    public async Task<int> ObtenerCopiasDisponiblesAsync(string isbn)
    {
        var libro = await _context.Libros.FindAsync(isbn);
        if (libro == null) return 0;

        var prestamosActivos = await _context.Prestamos
            .CountAsync(p => p.LibroId == isbn && p.Estado == 1);

        return libro.CantCopias - prestamosActivos;
    }

    public async Task<bool> ValidarSocioActivoAsync(int nroSocio)
    {
        var socio = await _context.Socios.FindAsync(nroSocio);
        return socio != null && socio.Activo == 1;
    }

    public async Task<bool> ValidarMultasPendientesAsync(int nroSocio)
    {
        var multa = await _context.Multas.FindAsync(nroSocio);
        return multa == null || multa.Valor <= 0;
    }

    public async Task<bool> ValidarLimitePrestamosAsync(int nroSocio)
    {
        var socio = await _context.Socios
            .Include(s => s.TipoSocioNavigation)
            .FirstOrDefaultAsync(s => s.NroSocio == nroSocio);

        if (socio == null) return false;

        var prestamosActivos = await _context.Prestamos
            .CountAsync(p => p.SocioId == nroSocio && p.Estado == 1);

        return prestamosActivos < socio.TipoSocioNavigation.MaxLibrosSimutaneos;
    }

    public async Task<List<Libro>> BuscarLibrosAsync(string texto)
    {
        return await _context.Libros
            .Where(l => l.Titulo.Contains(texto) || l.Autor.Contains(texto))
            .ToListAsync();
    }

    public async Task<DateTime> CalcularFechaVencimientoAsync(int nroSocio)
    {
        var socio = await _context.Socios
            .Include(s => s.TipoSocioNavigation)
            .FirstOrDefaultAsync(s => s.NroSocio == nroSocio);

        if (socio == null)
            throw new Exception("Socio no encontrado");

        return DateTime.Now.AddDays(socio.TipoSocioNavigation.DiasPrestamo);
    }

    public async Task<bool> PrestarLibroAsync(int nroSocio, string isbn)
    {
        if (!await ValidarSocioActivoAsync(nroSocio))
            throw new Exception("RN-01: El socio está inactivo");

        if (!await ValidarMultasPendientesAsync(nroSocio))
            throw new Exception("RN-02: El socio tiene multas pendientes");

        if (!await ValidarLimitePrestamosAsync(nroSocio))
            throw new Exception("RN-04: El socio superó el límite de préstamos");

        var copiasDisponibles = await ObtenerCopiasDisponiblesAsync(isbn);
        if (copiasDisponibles <= 0)
            throw new Exception("RN-03: No hay copias disponibles del libro");

        var fechaVencimiento = await CalcularFechaVencimientoAsync(nroSocio);

        var prestamo = new Prestamo
        {
            SocioId = nroSocio,
            LibroId = isbn,
            FechaPrestamo = FormatearFecha(DateTime.Now),
            FechaVencimiento = FormatearFecha(fechaVencimiento),
            Estado = 1
        };

        _context.Prestamos.Add(prestamo);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DevolverLibroAsync(int nroSocio, string isbn)
    {
        var prestamo = await _context.Prestamos
            .FirstOrDefaultAsync(p => p.SocioId == nroSocio && p.LibroId == isbn && p.Estado == 1);

        if (prestamo == null)
            throw new Exception("No se encontró un préstamo activo para este libro");

        var fechaDevolucion = DateTime.Now;
        prestamo.FechaDevolucion = FormatearFecha(fechaDevolucion);

        var fechaVencimiento = ParsearFecha(prestamo.FechaVencimiento);
        if (fechaDevolucion > fechaVencimiento)
        {
            var diasDemora = (fechaDevolucion - fechaVencimiento).Days;
            var socio = await _context.Socios
                .Include(s => s.TipoSocioNavigation)
                .FirstOrDefaultAsync(s => s.NroSocio == nroSocio);

            if (socio != null)
            {
                var multaValor = diasDemora * socio.TipoSocioNavigation.MultaPorDia;

                var multa = await _context.Multas.FindAsync(nroSocio);
                if (multa == null)
                {
                    multa = new Multa { SocioId = nroSocio, Valor = multaValor };
                    _context.Multas.Add(multa);
                }
                else
                {
                    multa.Valor += multaValor;
                }

                Console.WriteLine($"⚠️ Demora de {diasDemora} días. Multa: ${multaValor}");
            }
        }

        prestamo.Estado = 2; 
        await _context.SaveChangesAsync();

        await ProcesarReservasAsync(isbn);

        return true;
    }

    public async Task ProcesarReservasAsync(string isbn)
    {
        var reserva = await _context.Reservas
            .Where(r => r.LibroId == isbn && r.Estado == 1)
            .OrderBy(r => r.FechaReserva)
            .FirstOrDefaultAsync();

        if (reserva != null)
        {
            reserva.Estado = 2;
            await _context.SaveChangesAsync();

            var socio = await _context.Socios.FindAsync(reserva.SocioId);
            Console.WriteLine($"📢 Notificación: La reserva de {socio?.Nombre} para el libro {isbn} ha sido cumplida");
        }
    }

    public async Task<bool> ReservarLibroAsync(int nroSocio, string isbn)
    {
        if (!await ValidarSocioActivoAsync(nroSocio))
            throw new Exception("RN-01: El socio está inactivo");

        var reservaExistente = await _context.Reservas
            .FirstOrDefaultAsync(r => r.SocioId == nroSocio && r.LibroId == isbn && r.Estado == 1);

        if (reservaExistente != null)
            throw new Exception("RN-08: Ya tienes una reserva activa para este libro");

        var reserva = new Reserva
        {
            SocioId = nroSocio,
            LibroId = isbn,
            FechaReserva = FormatearFecha(DateTime.Now),
            Estado = 1 
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Socio?> ObtenerSocioAsync(int nroSocio)
    {
        return await _context.Socios
            .Include(s => s.TipoSocioNavigation)
            .Include(s => s.Prestamos.Where(p => p.Estado == 1))
            .Include(s => s.Multa)
            .FirstOrDefaultAsync(s => s.NroSocio == nroSocio);
    }

    public async Task<List<Prestamo>> ObtenerPrestamosActivosAsync(int nroSocio)
    {
        return await _context.Prestamos
            .Include(p => p.Libro)
            .Where(p => p.SocioId == nroSocio && p.Estado == 1)
            .ToListAsync();
    }

    public async Task<decimal> CalcularMultaPendienteAsync(int nroSocio)
    {
        var multa = await _context.Multas.FindAsync(nroSocio);
        return multa?.Valor ?? 0;
    }

    public async Task<List<Libro>> ObtenerLibrosDisponiblesAsync()
    {
        var libros = await _context.Libros.ToListAsync();
        var disponibles = new List<Libro>();

        foreach (var libro in libros)
        {
            var copiasDisponibles = await ObtenerCopiasDisponiblesAsync(libro.ISBN);
            if (copiasDisponibles > 0)
                disponibles.Add(libro);
        }

        return disponibles;
    }
}