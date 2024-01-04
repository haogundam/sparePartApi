using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);

        Task<User> GetEmail(string email);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        void UpdateUser(User user);
    }
}
