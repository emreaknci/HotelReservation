using Core.Entities;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Utils.Security.JWT
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration Configuration;
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = new()
            {
                Issuer = Configuration["TokenOptions:Issuer"],
                Audience = Configuration["TokenOptions:Audience"],
                SecurityKey = Configuration["TokenOptions:SecurityKey"],
                AccessTokenExpiration = Convert.ToInt16(Configuration["TokenOptions:AccessTokenExpiration"])
            };
        }

        public AccessToken CreateAccessToken(AppUser user)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new()
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        private IEnumerable<Claim> SetClaims(AppUser user)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email!);
            claims.AddName($"{user.FirstName} {user.LastName}");
            //claims.AddRoles(roles);
            claims.AddUserType(user.UserType);
            return claims;
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, AppUser user, SigningCredentials signingCredentials)
        {
            JwtSecurityToken jwt = new(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user),
                signingCredentials: signingCredentials
            );
            return jwt;
        }


    }
}
