using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class ListCpf
{
    public int ListId { get; set; }

    public string BussinesBussCnpj { get; set; } = null!;

    public string ListTipo { get; set; } = null!;

    public virtual Bussines BussinesBussCnpjNavigation { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = [];
}
