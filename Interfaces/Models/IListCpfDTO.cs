using Api.Domain;

public interface IListCpf
{
    int ListId { get; set; }

    string BussinesBussCnpj { get; set; }

    string ListTipo { get; set; }

    Bussines BussinesBussCnpjNavigation { get; set; }

    ICollection<User> Users { get; set; }
}