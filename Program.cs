using Biblioteca_Monzon_Randisi.Data;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Monzon_Randisi;

class Program
{
    static void Main(string[] args)
    {
        using var context = new BibliotecaContext();

        context.Database.EnsureCreated();

        Console.WriteLine("=== LIBROS DISPONIBLES EN LA BIBLIOTECA ===\n");

        var librosDisponibles = context.Libros
            .Where(l => l.CantCopias > 0)
            .OrderBy(l => l.Titulo)
            .ToList();

        if (librosDisponibles.Any())
        {
            foreach (var libro in librosDisponibles)
            {
                Console.WriteLine($"📚 {libro.Titulo}");
                Console.WriteLine($"   Autor: {libro.Autor}");
                Console.WriteLine($"   ISBN: {libro.ISBN}");
                Console.WriteLine($"   Copias disponibles: {libro.CantCopias}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No hay libros disponibles en este momento.");
        }

        Console.WriteLine("Presione cualquier tecla para salir...");
        Console.ReadKey();
    }
}