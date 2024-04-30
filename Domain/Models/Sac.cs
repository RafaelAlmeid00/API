using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class Sac
{
    public string SacTicket { get; set; } = null!;

    public DateOnly SacData { get; set; }

    public string UserUserCpf { get; set; } = null!;

    public virtual ICollection<SacMessage> SacMessages { get; set; } = [];

    public virtual User UserUserCpfNavigation { get; set; } = null!;
}
