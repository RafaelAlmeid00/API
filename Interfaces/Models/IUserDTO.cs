using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.Interface
{
    public interface IUserDTO
    {
        [Key]
        [MaxLength(11)]
        string? UserCpf { get; set; }
        [MaxLength(9)]
        string? UserRg { get; set; }

        string? UserNome { get; set; }

        string? UserEmail { get; set; }

        string? UserSenha { get; set; }

        DateOnly UserNascimento { get; set; }

        byte[]? UserFotoPerfil { get; set; }

        byte[]? UserRgfrente { get; set; }

        byte[]? UserRgtras { get; set; }

        string? UserEndCep { get; set; }

        string? UserEndUf { get; set; }

        string? UserEndbairro { get; set; }

        string? UserEndrua { get; set; }

        string? UserEndnum { get; set; }

        string? UserEndcomplemento { get; set; }

        string? UserEndcidade { get; set; }

        string? UserTipo { get; set; }

        int? ListCpfListId { get; set; }
        string? UserStatus { get; set; }

    }

    public interface IUserLoginDTO
    {
        [Key]
        [MaxLength(11, ErrorMessage = "CPF não pode ter mais que 11 dígitos")]
        string UserCpf { get; set; }
        [PasswordPropertyText]
        string UserSenha { get; set; }
    }
}