using Api.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Services
{
    public partial class Auth : IAuth
    {

        public string CreateTokenBussines(IBussinesDTO data)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("BussinesCNPJ", data.BussCnpj ?? ""),
                    new Claim("Email", data.BussEmail ?? ""),
                    new Claim("Senha", data.BussSenha ?? ""),
                    new Claim("Nome", data.BussNome ?? ""),
                    new Claim("FotoPerfil", data.BussFotoPerfil?.ToString() ?? ""),
                    new Claim("Tipo", data.BussTipo?.ToString() ?? ""),
                    new Claim("EndCep", data.BussEndCep?.ToString() ?? ""),
                    new Claim("EndUf", data.BussEndUf?.ToString() ?? ""),
                    new Claim("Endbairro", data.BussEndbairro?.ToString() ?? ""),
                    new Claim("Endrua", data.BussEndrua?.ToString() ?? ""),
                    new Claim("Endnum", data.BussEndnum?.ToString() ?? ""),
                    new Claim("EndCidade", data.BussEndcidade?.ToString() ?? ""),
                    new Claim("Status", data.BussStatus?.ToString() ?? "")
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
