using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IUserRepository
    {
        // Authenticate user
        Task<User> AuthenticateAsync(string username, string password);
    }
}
