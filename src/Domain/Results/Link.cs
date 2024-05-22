// Domain/Results/Link.cs
using Api.Interface;

namespace Api.Domain.Results
{
    public record Link : ILink
    {        
        public required string Rel { get; set; }
        public required string Href { get; set; }
        public required string Method { get; set; }
    }
}