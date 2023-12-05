using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Service.Contract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Service.Service
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// This method creates an access token based on the id
        /// </summary>
        /// <param name="jwtOptions">jwt options</param>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public string CreateAccessToken(JwtOptions jwtOptions, string id)
        {
            var tokenExpiration = TimeSpan.FromSeconds(jwtOptions.ExpirationSeconds);

            var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("sub", id),
                new Claim("name", id),
                new Claim("aud", jwtOptions.Audience)
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.Add(tokenExpiration),
                signingCredentials: signingCredentials);

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rawToken;
        }
    }
}
