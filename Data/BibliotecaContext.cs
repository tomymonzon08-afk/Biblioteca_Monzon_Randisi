using Biblioteca_Monzon_Randisi.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Monzon_Randisi.Data;

public class BibliotecaContext : DbContext
{
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Socio> Socios { get; set; }
    public DbSet<TipoSocio> TiposSocios { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<LibrosGeneros> LibrosGeneros { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<EstadoPrestamo> EstadosPrestamo { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<EstadoReserva> EstadosReserva { get; set; }
    public DbSet<Multa> Multas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Biblioteca.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}