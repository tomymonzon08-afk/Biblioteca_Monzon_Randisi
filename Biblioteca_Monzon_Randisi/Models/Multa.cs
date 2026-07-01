using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_Monzon_Randisi.Models;

public class Multa
{
    [Key]
    [ForeignKey(nameof(Socio))]
    public int SocioId { get; set; }
    public int Valor { get; set; }
    public virtual Socio Socio { get; set; } = null!;
}