using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public UserRepository (LumberStoreSystemDbContext context)
        {
            _context = context;
        }
        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(u => u.Address).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.Include(u=> u.Address).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(u => u.Address) 
                .FirstOrDefaultAsync(u => u.Email == username);
        }
    }
}
