using Api.Domain;

public interface IListCpfDTO
{
    int? ListId { get; set; }

    string? BussinesBussCnpj { get; set; }

    string? ListTipo { get; set; }
    string? ListCPF { get; set; }

    Bussines? BussinesBussCnpjNavigation { get; set; }

    ICollection<User>? Users { get; set; }
}