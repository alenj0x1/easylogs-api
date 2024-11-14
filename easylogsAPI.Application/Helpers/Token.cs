using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using easylogsAPI.Application.Interfaces.Helpers;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Models.Helpers;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace easylogsAPI.Application.Helpers;

public class Token(IConfiguration configuration) : IToken
{
    private readonly IConfiguration _config = configuration;
    
    public CreateAccessTokenData? CreateAccessToken(Userapp userapp, DateTime expiration)
    {
        try
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("UserId", userapp.UserAppId.ToString()));

            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtSecretKey)));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                audience: _config["Jwt:Audience"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtAudience),
                issuer: _config["Jwt:Issuer"] ?? throw new Exception(ResponseConsts.ConfigurationMissingJwtIssuer),
                claims: claims.Claims,
                expires: expiration,
                signingCredentials: signingCredentials);
        
            return new CreateAccessTokenData()
            {
                Value = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
}