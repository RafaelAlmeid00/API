using Api.Interface;

namespace Api.Domain
{
    public record ResultadoOperacao<T> : IResultadoOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string? Erro { get; set; }
        public T? Data { get; set; }
        public required ILink Link { get; set; }

    }

    // separar camadas

    public record Link : ILink
    {        
        public required string Rel { get; set; }
        public required string Href { get; set; }
        public required string Method { get; set; }
    }
}
