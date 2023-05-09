using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class FilmeController : ControllerBase
{

    //private static List<Filme> filmes = new List<Filme>();
    //private static int id = 0;

    // Utilizando o Banco:
    private FilmeContext _context;

    // Utilizando AutoMapper
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // DOCUMENTAÇÃO SWAGGER
    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost] // não estamos recuperando, e sim adicionando, postando
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] 
    /*Filme filme*/ CreateFilmeDto filmeDto) // O filme vem do corpo da requisição
    {
        // Convertendo o DTO recebido para um Filme
        Filme filme = _mapper.Map<Filme>(filmeDto);
        // Mapeamento para um Filme (model) a partir de um filmeDto (DTO).

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        //filme.Id = id++;
        //filmes.Add(filme);

        // Retornando o recurso que foi acabado de criar
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id}, filme); 
    }

    /// <summary>
    /// Exibição dos filmes do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet] // recuperando informações
    public IEnumerable<ReadFilmeDto>/*<Filme>*/ RecuperaFilmes([FromQuery] int skip = 0, 
     [FromQuery] int take = 10) // buscando as informações de input
                                     // declarando padrões para skip e take, caso o usuário não passe parâmetros
    {
        // FromQuery (link): https://site.com/filme?skip=[valor]&take=[valor]

        // IEnumerable:
        // Posteriormente, se a implementação da nossa lista for alterada e deixar de utilizar a classe List<>
        // por outra classe que implemente IEnumerable, não precisaremos trocar a assinatura do nosso método.

        //return filmes;

        // Conceito de Paginação
        //return filmes.Skip(skip).Take(take);
        //return _context.Filmes.Skip(skip).Take(take);
        // Pular n elementos, pegar n elementos.

        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip
            (skip).Take(take));
    }

    // Os get's são diferenciados por PARÂMETRO.
    // Caso haja, será executado o de busca por ID.
    // Se não, será executado "RecuperaFilmes".

    // Parâmetro no link: https://site.com/filme/[parametro]
    // EX: https://site.com/filme/1 -> id = 1
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id) // Antes: "Filme?" -> "?": pode ser nulo ou não
        // IActionResult: resultado de uma ação encontrada (nesse caso, "Ok" ou "NotFound").
    {
        //var filme = filmes.FirstOrDefault(filme => filme.Id == id);
        
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        // Para cada elemento da lista de filmes, o id desse filme deve ser igual ao id passado por parâmetro.
        // Se não houver, retornará o valor DEFAULT (nulo).

        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filme);
    }

    // PUT: atualizar o objeto INTEIRO
    // PATCH: atualizações PARCIAIS
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, 
        [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);

        if (filme == null) return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        // Status de ATUALIZAÇÃO
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, 
        JsonPatchDocument<UpdateFilmeDto> patch)
        // Lib "NewtonSoft"
        // Contêm as informações de "UpdateFilmeDto"
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        // Status de ATUALIZAÇÃO
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);

        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
