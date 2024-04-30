using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class DriverBus
{
    public string DriverCpf { get; set; } = null!;

    public string DriverRg { get; set; } = null!;

    public string DriverNome { get; set; } = null!;

    public DateOnly DriverNascimento { get; set; }

    public DateOnly DriverAdmissao { get; set; }

    public DateOnly? DriverDemissao { get; set; }

    public virtual ICollection<TurnBus> TurnBus { get; set; } = [];
}
