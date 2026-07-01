using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class Socio
{
    [Key]
    public int NroSocio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TipoSocio { get; set; }
    public int Activo { get; set; }

    public virtual TipoSocio TipoSocioNavigation { get; set; } = null!;
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public virtual Multa? Multa { get; set; }
}