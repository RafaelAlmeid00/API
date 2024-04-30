using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class TurnBus
{
    public int TurnId { get; set; }

    public DateTime TurnInicio { get; set; }

    public DateTime TurnFim { get; set; }

    public string DriverBusDriverCpf { get; set; } = null!;

    public int BussBusId { get; set; }

    public virtual Buss BussBus { get; set; } = null!;

    public virtual DriverBus DriverBusDriverCpfNavigation { get; set; } = null!;

    public virtual ICollection<ValidationCard> ValidationCards { get; set; } = [];
}
