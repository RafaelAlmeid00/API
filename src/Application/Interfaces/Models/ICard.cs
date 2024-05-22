using Api.Domain;

namespace Api.Interface;

public interface ICard
{
    public int CardId { get; set; }

    public string CardValidade { get; set; }

    public string CardSaldo { get; set; }

    public string CardTipo { get; set; }

    public string? CardStatus { get; set; }

    public DateTime? CardUltimoUso { get; set; }

    public int? CardUltimoOnibus { get; set; }

    public int RequestCardReqId { get; set; }
    
}