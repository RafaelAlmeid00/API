using Api.Domain;
using Api.Infrastructure;
using Api.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Api.Domain.Results;

namespace Api.Adapters_Repository;

public class CardRepository(EasyPassContext context) : IRepositoryCard
{
    private readonly EasyPassContext _context = context;

    // Podemos adicionar um card number público pra não tem que adicionar o Id pra verificação
    // e depois substituir os cardTipo por CardNumber por exemplo

    public async Task<IResultadoOperacao<List<Card>>> Search(Card data)
    {
        ILink link = new Link { Rel = "search_card", Href = "/Card", Method = "GET" };

        try
        {
            IQueryable<Card> queryable = _context.Cards.AsQueryable();

            IQueryable<Card> filteredQueryable = queryable
                .Where(a => data.CardId == null || a.CardId == data.CardId)
                .Where(a => string.IsNullOrEmpty(data.CardTipo) || a.CardTipo == data.CardTipo);

            List<Card> cards = await filteredQueryable.ToListAsync();
            return cards.Count == 0
                ? new ResultadoOperacao<List<Card>> { Sucesso = false, Erro = "Nenhum Card encontrado", Link = link }
                : new ResultadoOperacao<List<Card>> { Data = cards, Sucesso = true, Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<List<Card>> { Sucesso = false, Erro = $"Erro ao buscar Cards: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<Card>> Create(Card data)
    {
        ILink link = new Link { Rel = "create_card", Href = "/Card", Method = "POST" };

        try
        {
            await _context.Cards.AddAsync(data);
            await _context.SaveChangesAsync();

            return new ResultadoOperacao<Card> { Data = data, Sucesso = true, Link = link };
        }
        catch (DbUpdateException)
        {
            return await CardExists(data.CardTipo)
                ? new ResultadoOperacao<Card> { Sucesso = false, Erro = "Card já existe", Link = link }
                : new ResultadoOperacao<Card> { Sucesso = false, Erro = "Erro ao salvar Card", Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<Card> { Sucesso = false, Erro = $"Erro ao salvar Card: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<Card>> GetOne(Card data)
    {
        ILink link = new Link { Rel = "get_card", Href = $"/Card/{data.CardTipo}", Method = "GET" };

        try
        {
            Card? card = await _context.Cards.FindAsync(data.CardTipo);
            return card is not null
                ? new ResultadoOperacao<Card> { Data = card, Sucesso = true, Link = link }
                : new ResultadoOperacao<Card> { Sucesso = false, Erro = "Card não existe", Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<Card> { Sucesso = false, Erro = $"Erro ao buscar Card: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<Card>> Edit(Card data)
    {
        ILink link = new Link { Rel = "edit_card", Href = "/Card", Method = "PUT" };

        try
        {
            Card? card = await _context.Cards.FindAsync(data.CardTipo);
            if (card is not null)
            {
                UpdateEntityProperties(card, data);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Card> { Data = card, Sucesso = true, Link = link };
            }

            return new ResultadoOperacao<Card> { Sucesso = false, Erro = "Card não existe", Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<Card> { Sucesso = false, Erro = $"Erro ao editar Card: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<Card>> Delete(Card data)
    {
        ILink link = new Link { Rel = "delete_card", Href = "/Card", Method = "DELETE" };

        try
        {
            Card? card = await _context.Cards.FindAsync(data.CardTipo);
            if (card == null)
            {
                return new ResultadoOperacao<Card> { Sucesso = false, Erro = "Card não existe", Link = link };
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return new ResultadoOperacao<Card> { Data = card, Sucesso = true, Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<Card> { Sucesso = false, Erro = $"Erro ao deletar Card: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<Card>> AlterType(Card data)
    {
        ILink link = new Link { Rel = "alter_type_card", Href = "/Card/AlterType", Method = "POST" };

        try
        {
            IResultadoOperacao<Card> cardResult = await GetOne(data);
            if (cardResult.Data is not null)
            {
                cardResult.Data.CardTipo = data.CardTipo;
                _context.Cards.Update(cardResult.Data);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Card> { Data = cardResult.Data, Sucesso = true, Link = link };
            }

            return new ResultadoOperacao<Card> { Sucesso = false, Erro = "Card não existe", Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<Card> { Sucesso = false, Erro = $"Erro ao alterar tipo do Card: {ex.Message}", Link = link };
        }
    }

    public async Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data)
    {
        ILink link = new Link { Rel = "create_card_with_excel", Href = "/Card/CreateWithExcel", Method = "POST" };

        List<dynamic> itensSaved = new();
        List<dynamic> result = new List<dynamic>
        {
            new { saved = itensSaved, valids = data.Item1, invalids = data.Item2 }
        };

        try
        {
            foreach (var item in data.Item1)
            {
                await _context.Cards.AddAsync(item);
                await _context.SaveChangesAsync();
                itensSaved.Add(item);
            }

            return itensSaved.Count != 0
                ? new ResultadoOperacao<dynamic> { Data = result, Sucesso = true, Link = link }
                : new ResultadoOperacao<dynamic> { Data = result, Sucesso = false, Erro = "Erro ao salvar Cards", Link = link };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao<dynamic> { Data = result, Sucesso = false, Erro = $"Erro ao salvar Cards: {ex.Message}", Link = link };
        }
    }

    private async Task<bool> CardExists(string cardNumber)
    {
        return await _context.Cards.AnyAsync(e => e.CardTipo == cardNumber);
    }

    private void UpdateEntityProperties(Card target, Card source)
    {
        var properties = typeof(Card).GetProperties();
        foreach (var property in properties)
        {
            if (property.CanWrite)
            {
                var value = property.GetValue(source);
                if (value != null)
                {
                    property.SetValue(target, value);
                }
            }
        }
    }
}
