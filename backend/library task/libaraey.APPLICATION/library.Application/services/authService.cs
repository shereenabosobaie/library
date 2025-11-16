using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace library_task.library.Application.services
{
    public class authService : IauthService
    {
        private readonly IConfiguration _configuration;

        public authService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string createtoken(member mem)
        {
            
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, mem.Id.ToString()),
                    new Claim(ClaimTypes.Name, mem.Name),
                    new Claim(ClaimTypes.Email, mem.Email),
                    new Claim(ClaimTypes.Role, mem.role)
                };
                var tokenKey = _configuration["AppSettings:Token"];

                Console.WriteLine("Token Key: '" + _configuration["AppSettings:Token"] + "'");
                

                if (string.IsNullOrEmpty(tokenKey))
                {
                    throw new Exception("JWT Token key is not configured");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["AppSettings:Issuer"] ?? "LibraryAPI",
                    audience: _configuration["AppSettings:Audience"] ?? "LibraryUsers",
                    
                    
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Token creation failed: {ex.Message}");
            }
        }
        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                return userId;
            }
            catch
            {
                
                return null;
            }
        }
    }
}