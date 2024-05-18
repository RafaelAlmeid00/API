using System;
using System.Collections.Generic;

namespace Api.Domain;

public record Card
{
    public int CardId { get; set; }

    public string CardValidade { get; set; } = null!;

    public string CardSaldo { get; set; } = null!;

    public string CardTipo { get; set; } = null!;

    public string? CardStatus { get; set; }

    public DateTime? CardUltimoUso { get; set; }

    public int? CardUltimoOnibus { get; set; }

    public int RequestCardReqId { get; set; }

    public virtual RequestCard RequestCardReq { get; set; } = null!;

    public virtual ICollection<ValidationCard> ValidationCards { get; set; } = [];
}
