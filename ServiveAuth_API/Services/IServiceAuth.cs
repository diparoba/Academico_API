//Inteface for Auth
using ServiceAuth_API.Models;

namespace ServiceAuth_API.Services
{
    public interface IServiceAuth
    {
        Task<User> RegisterAsync(User user);
        Task<User> LoginAsync(User user);
        Object GenerateToken(User user);
        
    }
}