
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Interface;

namespace Api.Domain;

public record User : IUserDTO
{
    [Key]
    [MinLength(11, ErrorMessage = "CPF não pode ter menos que 11 dígitos")]
    public string? UserCpf { get; set; }
    [MinLength(9, ErrorMessage = "RG não pode ter menos que 9 dígitos")]
    public string? UserRg { get; set; }
    public string? UserNome { get; set; }
    [EmailAddress]
    public string? UserEmail { get; set; }
    [PasswordPropertyText]
    public string? UserSenha { get; set; }
    public DateOnly UserNascimento { get; set; }
    public byte[]? UserFotoPerfil { get; set; }

    public byte[]? UserRgfrente { get; set; }

    public byte[]? UserRgtras { get; set; }
    [MaxLength(8, ErrorMessage = "CEP não pode ter mais que 8 dígitos")]
    public string? UserEndCep { get; set; }
    [MaxLength(2, ErrorMessage = "UF não pode ter mais que 2 dígitos")]
    public string? UserEndUf { get; set; }
    public string? UserEndbairro { get; set; }
    public string? UserEndrua { get; set; }
    public string? UserEndnum { get; set; }
    public string? UserEndcomplemento { get; set; }

    public string? UserEndcidade { get; set; }
    public string? UserTipo { get; set; }
    public int? ListCpfListId { get; set; }
    public virtual ListCpf? ListCpfList { get; set; }
    public string? UserStatus { get; set; }

    public virtual ICollection<RequestCard> RequestCards { get; set; } = [];

    public virtual ICollection<SacMessage> SacMessages { get; set; } = [];

    public virtual ICollection<Sac> Sacs { get; set; } = [];
}

public record UserLogin : IUserLoginDTO
{
    [Key]
    [MaxLength(11, ErrorMessage = "CPF não pode ter mais que 11 dígitos")]
    public required string UserCpf { get; set; }
    [PasswordPropertyText]
    public required string UserSenha { get; set; }

}