using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Interface;

namespace Api.Domain;

public record Admin : IAdminDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? AdmId { get; set; }
    public string? AdmNome { get; set; }
    [EmailAddress]
    public string? AdmEmail { get; set; }
    [PasswordPropertyText]
    public string? AdmSenha { get; set; }
    public int? AdmLevel { get; set; }
    public virtual ICollection<SacMessage>? SacMessages { get; set; } = [];

    public static explicit operator Admin(List<Admin> v)
    {
        throw new NotImplementedException();
    }
}

public record AdminLogin : IAdminLoginDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Insira um Email")]
    public required string AdmEmail { get; set; }
    [PasswordPropertyText]
    [Required(ErrorMessage = "Insira uma Senha")]
    public required string AdmSenha { get; set; }
}