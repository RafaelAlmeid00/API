namespace Api.Interface
{
    public interface IBussinesDTO
    {
        string BussCnpj { get; set; }

        string BussNome { get; set; }

        string BussEmail { get; set; }

        string BussSenha { get; set; }

        string BussContato { get; set; }

        public byte[]? BussFotoPerfil { get; set; }

        string BussEndCep { get; set; }

        string BussEndUf { get; set; }

        string BussEndrua { get; set; }

        string BussEndnum { get; set; }

        public string? BussEndcomplemento { get; set; }

        string BussEndcidade { get; set; }

        string BussTipo { get; set; }
    }
}