using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class Bussines
{
    public string BussCnpj { get; set; } = null!;

    public string BussNome { get; set; } = null!;

    public string BussEmail { get; set; } = null!;

    public string BussSenha { get; set; } = null!;

    public string BussContato { get; set; } = null!;

    public byte[]? BussFotoPerfil { get; set; }

    public string BussEndCep { get; set; } = null!;

    public string BussEndUf { get; set; } = null!;

    public string BussEndrua { get; set; } = null!;

    public string BussEndnum { get; set; } = null!;

    public string? BussEndcomplemento { get; set; }

    public string BussEndcidade { get; set; } = null!;

    public string BussTipo { get; set; } = null!;

    public virtual ICollection<Buss> Busses { get; set; } = [];

    public virtual ICollection<ListCpf> ListCpfs { get; set; } = [];
}
