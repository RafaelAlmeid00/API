using Api.Interface;

namespace Api.Domain.Results
{
    public record ResultadoOperacao<T> : IResultadoOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string? Erro { get; set; }
        public T? Data { get; set; }
        public required ILink Link { get; set; }
    }
}