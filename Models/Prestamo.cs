using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_Monzon_Randisi.Models;

public class Prestamo
{
    [Key]
    [Column(Order = 0)]
    public int SocioId { get; set; }

    [Key]
    [Column(Order = 1)]
    public string LibroId { get; set; } = string.Empty;

    public string FechaPrestamo { get; set; } = string.Empty;
    public string FechaVencimiento { get; set; } = string.Empty;
    public string? FechaDevolucion { get; set; }
    public int Estado { get; set; }

    [ForeignKey(nameof(SocioId))]
    public virtual Socio Socio { get; set; } = null!;

    [ForeignKey(nameof(LibroId))]
    public virtual Libro Libro { get; set; } = null!;

    [ForeignKey(nameof(Estado))]
    public virtual EstadoPrestamo EstadoNavigation { get; set; } = null!;
}