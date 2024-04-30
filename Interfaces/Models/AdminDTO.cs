
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Domain;

namespace Api.Interface;
public interface IAdminDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    int? AdmId { get; set; }
    string? AdmNome { get; set; }
    [EmailAddress]
    string? AdmEmail { get; set; }
    [PasswordPropertyText]
    string? AdmSenha { get; set; }
    int? AdmLevel { get; set; }

    ICollection<SacMessage>? SacMessages { get; }
}

public interface IAdminLoginDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Insira um Email")]
    string AdmEmail { get; set; }
    [PasswordPropertyText]
    [Required(ErrorMessage = "Insira uma Senha")]
    string AdmSenha { get; set; }
}