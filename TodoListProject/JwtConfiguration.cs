using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoListProject.Models;

namespace TodoListProject
{
    public class JwtConfiguration
    {
        public readonly string key;
        public List<Users> users;
        DbContextClasse dbContact = new DbContextClasse();

        public JwtConfiguration(string key)
        {
            this.users = dbContact.Users.ToList();
            this.key = key;
        }

        public string authenticate(string email, string password)
        {
            if (!users.Any(e => e.Email == email && e.Password == password))
            {
                return null;
            }
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = new byte[16]; // 16 bytes = 128 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }
            var tokenKey = new SymmetricSecurityKey(keyBytes);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.Name, email)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    tokenKey,
                    SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
