using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class Libro
{
    [Key]
    public string ISBN { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int CantCopias { get; set; }

    public virtual ICollection<LibrosGeneros> LibrosGeneros { get; set; } = new List<LibrosGeneros>();
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}