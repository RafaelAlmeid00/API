using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Interface
{
    public interface IBussinesDTO
    {
        [Key]
        [MaxLength(14)]
        string BussCnpj { get; set; }

        string BussNome { get; set; }
        [EmailAddress]
        string BussEmail { get; set; }
        [PasswordPropertyText]
        string BussSenha { get; set; }

        string BussContato { get; set; }

        public byte[]? BussFotoPerfil { get; set; }

        string BussEndCep { get; set; }

        string BussEndUf { get; set; }

        string BussEndrua { get; set; }

        string BussEndnum { get; set; }

        public string? BussEndcomplemento { get; set; }

        string BussEndcidade { get; set; }
        string? BussEndbairro { get; set; }
        string BussTipo { get; set; }
        string? BussStatus { get; set; }
    }

    public interface IBussinesLoginDTO
    {
        [Key]
        [MaxLength(14)]
        [Required(ErrorMessage = "Insira o CNPJ")]
        string? BussCnpj { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "Insira a Senha")]
        string BussSenha { get; set; }

    }
}