using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class EstadoReserva
{
    [Key]
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}