using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class User
{
    public string UserCpf { get; set; } = null!;

    public string UserRg { get; set; } = null!;

    public string UserNome { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserSenha { get; set; } = null!;

    public DateOnly UserNascimento { get; set; }

    public byte[]? UserFotoPerfil { get; set; }

    public byte[]? UserRgfrente { get; set; }

    public byte[]? UserRgtras { get; set; }

    public string UserEndCep { get; set; } = null!;

    public string UserEndUf { get; set; } = null!;

    public string UserEndbairro { get; set; } = null!;

    public string UserEndrua { get; set; } = null!;

    public string UserEndnum { get; set; } = null!;

    public string UserEndcomplemento { get; set; } = null!;

    public string UserEndcidade { get; set; } = null!;

    public string UserTipo { get; set; } = null!;

    public int ListCpfListId { get; set; }

    public virtual ListCpf ListCpfList { get; set; } = null!;

    public virtual ICollection<RequestCard> RequestCards { get; set; } = [];

    public virtual ICollection<SacMessage> SacMessages { get; set; } = [];

    public virtual ICollection<Sac> Sacs { get; set; } = [];
}
