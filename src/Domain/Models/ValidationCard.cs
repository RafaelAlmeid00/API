using System;
using System.Collections.Generic;

namespace Api.Domain;

public record ValidationCard
{
    public int ValId { get; set; }

    public int ValOnibus { get; set; }

    public DateTime ValHorario { get; set; }

    public decimal ValGasto { get; set; }

    public int CardCardId { get; set; }

    public int TurnBusTurnId { get; set; }

    public virtual Card CardCard { get; set; } = null!;

    public virtual TurnBus TurnBusTurn { get; set; } = null!;
}
