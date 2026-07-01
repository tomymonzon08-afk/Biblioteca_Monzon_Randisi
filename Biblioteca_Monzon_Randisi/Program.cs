using Biblioteca_Monzon_Randisi.Data;
using Biblioteca_Monzon_Randisi.Services;
using Biblioteca_Monzon_Randisi.UI;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Monzon_Randisi;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new BibliotecaContext();

        var service = new BibliotecaService(context);
        var ui = new ConsoleUI(service, context);

        await ui.MostrarMenuPrincipalAsync();
    }
}