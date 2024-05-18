using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain;

public record ListCpf : IListCpf
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? ListId { get; set; }
    [ForeignKey("Bussines")]
    public string? BussinesBussCnpj { get; set; }
    public string? ListTipo { get; set; }
    public string? ListCPF { get; set; }
    public virtual Bussines? BussinesBussCnpjNavigation { get; set; }
    public virtual ICollection<User>? Users { get; set; } = [];
}
