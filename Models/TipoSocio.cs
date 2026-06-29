using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class TipoSocio
{
    [Key]
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int MaxLibrosSimutaneos { get; set; }
    public int DiasPrestamo { get; set; }
    public int MultaPorDia { get; set; }

    public virtual ICollection<Socio> Socios { get; set; } = new List<Socio>();
}