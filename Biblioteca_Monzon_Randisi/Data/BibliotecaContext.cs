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

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(@"Data Source=C:\Users\Estudiante\Documents\Politecnico general\Politecnico 6°\POO\Biblioteca_Monzon_Randisi\Biblioteca_Monzon_Randisi\DBBiblioteca.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ===== Nombres de TABLA reales (según script.sql) =====
        modelBuilder.Entity<Libro>().ToTable("Libro");
        modelBuilder.Entity<Socio>().ToTable("Socio");
        modelBuilder.Entity<TipoSocio>().ToTable("TipoSocio");
        modelBuilder.Entity<Genero>().ToTable("Generos");
        modelBuilder.Entity<LibrosGeneros>().ToTable("Libros_Generos");
        modelBuilder.Entity<Prestamo>().ToTable("Prestamo");
        modelBuilder.Entity<Reserva>().ToTable("Reserva");
        modelBuilder.Entity<EstadoPrestamo>().ToTable("Estado_prestamo");
        modelBuilder.Entity<EstadoReserva>().ToTable("Estado_reserva");
        modelBuilder.Entity<Multa>().ToTable("Multas");

        // ===== Libro =====
        modelBuilder.Entity<Libro>().Property(l => l.ISBN).HasColumnName("ISBN");
        modelBuilder.Entity<Libro>().Property(l => l.Titulo).HasColumnName("Titulo");
        modelBuilder.Entity<Libro>().Property(l => l.Autor).HasColumnName("Autor");
        modelBuilder.Entity<Libro>().Property(l => l.CantCopias).HasColumnName("CantCopias");

        // ===== Genero =====
        modelBuilder.Entity<Genero>().Property(g => g.GeneroId).HasColumnName("genero_id");
        modelBuilder.Entity<Genero>().Property(g => g.Nombre).HasColumnName("nombre");

        // ===== LibrosGeneros =====
        modelBuilder.Entity<LibrosGeneros>().Property(lg => lg.LibroId).HasColumnName("libro_id");
        modelBuilder.Entity<LibrosGeneros>().Property(lg => lg.GeneroId).HasColumnName("genero_id");

        // ===== Socio =====
        modelBuilder.Entity<Socio>().Property(s => s.NroSocio).HasColumnName("NroSocio");
        modelBuilder.Entity<Socio>().Property(s => s.Nombre).HasColumnName("Nombre");
        modelBuilder.Entity<Socio>().Property(s => s.Apellido).HasColumnName("Apellido");
        modelBuilder.Entity<Socio>().Property(s => s.Email).HasColumnName("Email");
        modelBuilder.Entity<Socio>().Property(s => s.TipoSocio).HasColumnName("tipoSocio");
        modelBuilder.Entity<Socio>().Property(s => s.Activo).HasColumnName("activo");

        // ===== TipoSocio =====
        modelBuilder.Entity<TipoSocio>().Property(t => t.Id).HasColumnName("id");
        modelBuilder.Entity<TipoSocio>().Property(t => t.Descripcion).HasColumnName("descripcion");
        modelBuilder.Entity<TipoSocio>().Property(t => t.MaxLibrosSimutaneos).HasColumnName("MaxLibrosSimutaneos");
        modelBuilder.Entity<TipoSocio>().Property(t => t.DiasPrestamo).HasColumnName("DiasPrestamo");
        modelBuilder.Entity<TipoSocio>().Property(t => t.MultaPorDia).HasColumnName("MultaPorDia");

        // ===== Multa =====
        modelBuilder.Entity<Multa>().Property(m => m.SocioId).HasColumnName("Socio_id");
        modelBuilder.Entity<Multa>().Property(m => m.Valor).HasColumnName("valor");
        modelBuilder.Entity<Multa>().Property(m => m.Estado).HasColumnName("estado");

        // ===== EstadoPrestamo / EstadoReserva =====
        modelBuilder.Entity<EstadoPrestamo>().Property(e => e.Id).HasColumnName("id");
        modelBuilder.Entity<EstadoPrestamo>().Property(e => e.Descripcion).HasColumnName("descripcion");

        modelBuilder.Entity<EstadoReserva>().Property(e => e.Id).HasColumnName("id");
        modelBuilder.Entity<EstadoReserva>().Property(e => e.Descripcion).HasColumnName("descripcion");

        // ===== Prestamo =====
        modelBuilder.Entity<Prestamo>().Property(p => p.SocioId).HasColumnName("Socio_id");
        modelBuilder.Entity<Prestamo>().Property(p => p.LibroId).HasColumnName("Libro_id");
        modelBuilder.Entity<Prestamo>().Property(p => p.FechaPrestamo).HasColumnName("FechaPrestamo");
        modelBuilder.Entity<Prestamo>().Property(p => p.FechaVencimiento).HasColumnName("FechaVencimiento");
        modelBuilder.Entity<Prestamo>().Property(p => p.FechaDevolucion).HasColumnName("FechaDevolucion");
        modelBuilder.Entity<Prestamo>().Property(p => p.Estado).HasColumnName("Estado");

        // ===== Reserva =====
        modelBuilder.Entity<Reserva>().Property(r => r.SocioId).HasColumnName("Socio_id");
        modelBuilder.Entity<Reserva>().Property(r => r.LibroId).HasColumnName("Libro_id");
        modelBuilder.Entity<Reserva>().Property(r => r.FechaReserva).HasColumnName("FechaReserva");
        modelBuilder.Entity<Reserva>().Property(r => r.Estado).HasColumnName("Estado");

        // ===== Claves primarias compuestas (igual que antes) =====
        modelBuilder.Entity<LibrosGeneros>()
            .HasKey(lg => new { lg.LibroId, lg.GeneroId });

        modelBuilder.Entity<Prestamo>()
            .HasKey(p => new { p.SocioId, p.LibroId });

        modelBuilder.Entity<Reserva>()
            .HasKey(r => new { r.SocioId, r.LibroId });

        // ===== Relaciones (igual que antes) =====
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