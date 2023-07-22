using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ServiceAuth_API.Models;
using ServiceAuth_API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceAuth_API
{
    public class ServiceAuth : IServiceAuth
    {
        private readonly MongoDBRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<User> _users;

        public ServiceAuth(IConfiguration configuration, MongoDBRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
            _users = _repository.database.GetCollection<User>("Users");
        }
        public object GenerateToken(User user)
        {
            //Header
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:Password"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _header = new JwtHeader(_signingCredentials);
            //Claims
            var _claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("lastname", user.Lastname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            //Payload
            var _payLoad = new JwtPayload(
                issuer: _configuration["JWT:Dominio"],
                audience: _configuration["JWT:appApi"],
                claims: _claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(60)
                );
            //Token
            var _token = new JwtSecurityToken(_header, _payLoad);
            return new JwtSecurityTokenHandler().WriteToken(_token);
        }

        public async Task<User> RegisterAsync(User user)
        {
            var userExists = await _users.Find(u => u.Email == user.Email).AnyAsync();
            if (userExists)
            {
                throw new Exception("User already exists");
            }
            user.Status = "A";
            user.CreatedAt = DateTime.UtcNow;
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> LoginAsync(User user)
        {
            var foundUser = await AuthAsync(user.Email, user.Password);

            if (foundUser == null)
            {
                throw new Exception("User not found");
            }
            return foundUser;
        }
        public async Task<User> AuthAsync(string username, string password)
        {
            try
            {
                var user = await _users.Find(u => u.Email.Equals(username)
                && u.Password.Equals(password)
                && u.Status.Equals("A")
                ).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}