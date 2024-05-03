using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Api.Interface;

namespace Api.Domain;

public partial class Bussines : IBussinesDTO
{
    public string? BussCnpj { get; set; }

    public string? BussNome { get; set; }

    public string? BussEmail { get; set; }

    public string? BussSenha { get; set; }

    public string? BussContato { get; set; }

    public byte[]? BussFotoPerfil { get; set; }

    public string? BussEndCep { get; set; }

    public string? BussEndUf { get; set; }

    public string? BussEndrua { get; set; }

    public string? BussEndnum { get; set; }

    public string? BussEndcomplemento { get; set; }

    public string? BussEndcidade { get; set; }
    public string? BussEndbairro { get; set; }

    public string? BussTipo { get; set; }
    public string? BussStatus { get; set; }

    public virtual ICollection<Buss>? Busses { get; set; } = [];

    public virtual ICollection<ListCpf>? ListCpfs { get; set; } = [];
}

public partial class BussinesLogin : IBussinesLoginDTO
{
    [Key]
    [MaxLength(14)]
    [Required(ErrorMessage = "Insira o CNPJ")]
    public required string BussCnpj { get; set; }
    [PasswordPropertyText]
    [Required(ErrorMessage = "Insira a Senha")]
    public required string BussSenha { get; set; }
}
