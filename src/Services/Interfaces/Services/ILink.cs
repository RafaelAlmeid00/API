namespace Api.Interface;

public interface ILink
{
    string Rel { get; set; }
    string Href { get; set; }
    string Method { get; set; }
}