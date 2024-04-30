using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class RequestCard
{
    public int ReqId { get; set; }

    public DateOnly ReqData { get; set; }

    public DateOnly? ReqEnvio { get; set; }

    public string ReqTipoCartao { get; set; } = null!;

    public string UserUserCpf { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = [];

    public virtual User UserUserCpfNavigation { get; set; } = null!;
}
