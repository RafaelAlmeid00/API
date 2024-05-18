using System;
using System.Collections.Generic;

namespace Api.Domain;

public record SacMessage
{
    public int SacmenId { get; set; }

    public string SacmenTexto { get; set; } = null!;

    public int? AdminAdmId { get; set; }

    public string? UserUserCpf { get; set; }

    public string SacSacTicket { get; set; } = null!;

    public virtual Admin? AdminAdm { get; set; }

    public virtual Sac SacSacTicketNavigation { get; set; } = null!;

    public virtual User? UserUserCpfNavigation { get; set; }
}
