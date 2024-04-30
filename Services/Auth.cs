using Api.Domain;
using Api.Interface;
using dotenv.net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public class Auth : IAuth<IAdminDTO>
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IDictionary<string, string> _envVariables;

        public Auth()
        {
            _envVariables = DotEnv.Read();
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_envVariables["key"]));
            _issuer = _envVariables["issuer"];
            _audience = _envVariables["audience"];
        }
        public string CreateToken(IAdminDTO adm)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("Id", adm.AdmId.ToString() ?? ""),
            new Claim("Email", adm.AdmEmail ?? ""),
            new Claim("Senha", adm.AdmSenha ?? ""),
            new Claim("Level", adm.AdmLevel.ToString() ?? "")
                ]),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
