using SparePart.ModelAndPersistance;

namespace SparePart.Services
{
    public interface IAuthService
    {
        string CreateToken(User user);

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        Task<User> GetUser(string email);

        void RegisterNewUser(User user);

        Task<bool> UserExists(string email);

    }
}
