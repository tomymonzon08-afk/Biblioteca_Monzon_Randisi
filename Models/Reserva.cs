using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_Monzon_Randisi.Models;

public class Reserva
{
    public int SocioId { get; set; }
    public string LibroId { get; set; } = string.Empty;
    public string FechaReserva { get; set; } = string.Empty;
    public int Estado { get; set; }

    [ForeignKey(nameof(SocioId))]
    public virtual Socio Socio { get; set; } = null!;

    [ForeignKey(nameof(LibroId))]
    public virtual Libro Libro { get; set; } = null!;

    [ForeignKey(nameof(Estado))]
    public virtual EstadoReserva EstadoNavigation { get; set; } = null!;
}