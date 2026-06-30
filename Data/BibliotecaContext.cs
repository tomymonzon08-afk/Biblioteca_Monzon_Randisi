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
        optionsBuilder.UseSqlite("Data Source=DBBiblioteca.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LibrosGeneros>()
            .HasKey(lg => new { lg.LibroId, lg.GeneroId });

        modelBuilder.Entity<Prestamo>()
            .HasKey(p => new { p.SocioId, p.LibroId });

        modelBuilder.Entity<Reserva>()
            .HasKey(r => new { r.SocioId, r.LibroId });

        modelBuilder.Entity<LibrosGeneros>()
            .HasOne(lg => lg.Libro)
            .WithMany(l => l.LibrosGeneros)
            .HasForeignKey(lg => lg.LibroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LibrosGeneros>()
            .HasOne(lg => lg.Genero)
            .WithMany(g => g.LibrosGeneros)
            .HasForeignKey(lg => lg.GeneroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Socio)
            .WithMany(s => s.Prestamos)
            .HasForeignKey(p => p.SocioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Libro)
            .WithMany(l => l.Prestamos)
            .HasForeignKey(p => p.LibroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.EstadoNavigation)
            .WithMany(e => e.Prestamos)
            .HasForeignKey(p => p.Estado)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Socio)
            .WithMany(s => s.Reservas)
            .HasForeignKey(r => r.SocioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Libro)
            .WithMany(l => l.Reservas)
            .HasForeignKey(r => r.LibroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.EstadoNavigation)
            .WithMany(e => e.Reservas)
            .HasForeignKey(r => r.Estado)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}