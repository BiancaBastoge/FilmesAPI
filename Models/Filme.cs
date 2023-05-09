using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

/* 
    > Ao invés de criar uma validação de "IS NOT NULL" na main, cria-se com [Required],
      exigindo que a informação a ser passada é obrigatória.
        * Cria-se "[Required]" ACIMA do campo escolhido.
    > Customizando MENSAGEM DE ERRO: (ErrorMessage).
    > MaxLength: definir um TAMANHO MÁXIMO a string.
    > Range: (início, fim)

    BANCO DE DADOS
    > Ferramentas -> NuGet -> Console
    > Executar comando: "Add-Migration CriandoTabelaDeFilme"
        * MIGRATION: mapeamento de dados para o banco de dados.
    > Executar comando> "Update-Database" (levar a database do migration para o MySql).
    
 */
public class Filme
{

    [Key] // indicando que o Id é a PK da tabela (banco)
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título do filme é obrigatório")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [MaxLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }

    [Required]
    [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]

    public int Duracao { get; set; }
}
