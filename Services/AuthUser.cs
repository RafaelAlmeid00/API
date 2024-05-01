using Api.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Services
{
    public partial class Auth : IAuth
    {

        public string CreateTokenUser(IUserDTO data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("UserCPF", data.UserCpf ?? ""),
                    new Claim("Email", data.UserEmail ?? ""),
                    new Claim("Senha", data.UserSenha ?? ""),
                    new Claim("RG", data.UserRg ?? ""),
                    new Claim("Nome", data.UserNome ?? ""),
                    new Claim("Nascimento", data.UserNascimento.ToString() ?? ""),
                    new Claim("FotoPerfil", data.UserFotoPerfil?.ToString() ?? ""),
                    new Claim("RGFrente", data.UserRgfrente?.ToString() ?? ""),
                    new Claim("RGTras", data.UserRgtras?.ToString() ?? ""),
                    new Claim("Tipo", data.UserTipo?.ToString() ?? ""),
                    new Claim("EndCep", data.UserEndCep?.ToString() ?? ""),
                    new Claim("EndUf", data.UserEndUf?.ToString() ?? ""),
                    new Claim("Endbairro", data.UserEndbairro?.ToString() ?? ""),
                    new Claim("Endrua", data.UserEndrua?.ToString() ?? ""),
                    new Claim("Endnum", data.UserEndnum?.ToString() ?? ""),
                    new Claim("EndCidade", data.UserEndcidade?.ToString() ?? ""),
                    new Claim("Status", data.UserStatus?.ToString() ?? "")
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
