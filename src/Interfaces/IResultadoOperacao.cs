using Api.Domain;

namespace Api.Interface
{
    public interface IResultadoOperacao<T>
    {
        T? Data { get; set; }
        string? Erro { get; set; }
        bool Sucesso { get; set; }
        ILink Link { get; set; }
    }

    public interface ILink
    {
        string Rel { get; set; }
        string Href { get; set; }
        string Method { get; set; }
    }
}
