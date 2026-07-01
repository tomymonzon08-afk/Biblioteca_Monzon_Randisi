using Biblioteca_Monzon_Randisi.Models;

namespace Biblioteca_Monzon_Randisi.Services;

public interface InterfazBiblioteca
{
    Task<bool> PrestarLibroAsync(int nroSocio, string isbn);
    Task<bool> DevolverLibroAsync(int nroSocio, string isbn);

    Task<bool> ReservarLibroAsync(int nroSocio, string isbn);
    Task ProcesarReservasAsync(string isbn);

    Task<List<Libro>> BuscarLibrosAsync(string texto);
    Task<Socio?> ObtenerSocioAsync(int nroSocio);
    Task<List<Prestamo>> ObtenerPrestamosActivosAsync(int nroSocio);
    Task<decimal> CalcularMultaPendienteAsync(int nroSocio);
    Task<List<Libro>> ObtenerLibrosDisponiblesAsync();

    Task<bool> ValidarSocioActivoAsync(int nroSocio);
    Task<bool> ValidarMultasPendientesAsync(int nroSocio);
    Task<bool> ValidarLimitePrestamosAsync(int nroSocio);
    Task<int> ObtenerCopiasDisponiblesAsync(string isbn);
}