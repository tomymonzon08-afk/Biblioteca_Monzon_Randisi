using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca_Monzon_Randisi.Models;

public class LibrosGeneros
{
    public string LibroId { get; set; } = string.Empty;
    public int GeneroId { get; set; }

    [ForeignKey(nameof(LibroId))]
    public virtual Libro Libro { get; set; } = null!;

    [ForeignKey(nameof(GeneroId))]
    public virtual Genero Genero { get; set; } = null!;
}