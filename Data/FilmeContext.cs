using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Data;

// Assim como a classe Controller estende de ControllerBase, no caso do FilmeContext
// precisamos informar que ele vai estender de DbContext
public class FilmeContext : DbContext
{
    // CONSTRUTOR
    // DbContextOptions: opções de acesso ao banco desse contexto.
    // Não faremos a utilização dele especificamente dentro desse construtor,
    // faremos a passagem dessas opções para o construtor da classe que estamos estendendo,
    // que é o próprio DbContext.
    public FilmeContext(DbContextOptions<FilmeContext> opts)
        : base (opts)
    {

    }

    public DbSet<Filme> Filmes { get; set; }
}
