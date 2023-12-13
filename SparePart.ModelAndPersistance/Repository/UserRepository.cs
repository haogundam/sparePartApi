using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SparePartContext _context;

        public UserRepository(SparePartContext sparePartContext)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
        }

        public Task<User> CreateUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return Task.FromResult(user);
        }

        public async Task<User> GetEmail(string email)
        {
            return await _context.Users.Where(c => c.Email == email).FirstOrDefaultAsync();
        }


    }
}
