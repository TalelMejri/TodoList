using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoListProject.Controllers;
using TodoListProject.Models;

namespace TodoListProject
{
    
    public class JwtConfiguration
    {
        public readonly string key;
        public List<Users> users;
        DbContextClasse dbContact = new DbContextClasse();
        PasswordHashing passwordHashing=new PasswordHashing();

        public JwtConfiguration(string key)
        {
            this.users = dbContact.Users.ToList();
            this.key = key;
        }

        public Users getUser(string email, string password)
        {
            Users user = dbContact.Users.Where(e => e.Email == email).FirstOrDefault();

            if (user != null && passwordHashing.VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public Users VerifyEmail(string email)
        {
            Users user = dbContact.Users.Where(e => e.Email == email).FirstOrDefault();
            return user;
        }

        public string authenticate(string email, string password)
        {
            Users user = dbContact.Users.Where(e => e.Email == email).FirstOrDefault();

            if (user != null && passwordHashing.VerifyPassword(password, user.Password))
            {
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
            else
            {
                return null;
            }
        }
    }
}
