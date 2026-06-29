using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class EstadoPrestamo
{
    [Key]
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}