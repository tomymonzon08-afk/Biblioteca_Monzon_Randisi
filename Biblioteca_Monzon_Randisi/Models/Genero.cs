using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Monzon_Randisi.Models;

public class Genero
{
    [Key]
    public int GeneroId { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public virtual ICollection<LibrosGeneros> LibrosGeneros { get; set; } = new List<LibrosGeneros>();
}