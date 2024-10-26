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
    public class OrderRepository : IOrderRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public OrderRepository(LumberStoreSystemDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.Include(o=>o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByClientId(int clientId)
        {
            return await _context.Orders
                        .Where(o => o.ClientId == clientId)
                        .Include(o => o.Items)
                        .Include(o => o.CuttingLists)
                        .ToListAsync();
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders =  await _context.Orders
                        .Include(o => o.Items)
                        .Include(o => o.CuttingLists)
                        .ToListAsync();

            return orders;
        }

    }
}
