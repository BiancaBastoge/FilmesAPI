using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

/*
    No modo anterior, utilizando a Model, o nosso "ID" estava sendo exposto ao usuário. 

    Não é uma prática muito boa deixarmos o nosso modelo de banco muito exposto, 
    principalmente na nossa camada de controlador. Porque essa é uma informação interna.

    Uma boa prática nesse caso é criar classes que vão instanciar objetos que são responsáveis 
    por trafegar dados em diferentes camadas.
    Como, por exemplo, a camada de apresentação, o HttpPost, que é onde o usuário interage com 
    nossa aplicação, e esse dado vai ser trafegado, por exemplo, para uma camada de persistência 
    que é onde vamos gravar o dado no banco.

    DTO (Data Transfer Object):
    > O controller agora utilizará do DTO, não da Model.
    > Define os acessos às informações as quais o usuário terá permissão.
    
 */

public class UpdateFilmeDto
{
    /*[Key] // indicando que o Id é a PK da tabela (banco)
    [Required]
    public int Id { get; set; }*/

    [Required(ErrorMessage = "O título do filme é obrigatório")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }

    [Required]
    [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]

    public int Duracao { get; set; }
}
