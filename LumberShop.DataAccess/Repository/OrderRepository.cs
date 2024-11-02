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
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving order: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(item => item.Product)
                .Include(o => o.CuttingLists)
                    .ThenInclude(cl => cl.Product)
                .Include(o => o.CuttingLists)
                    .ThenInclude(cl => cl.cuttingListItems) 
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByClientId(int clientId)
        {
            return await _context.Orders
                .Where(o => o.ClientId == clientId)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product) 
                .Include(o => o.CuttingLists)
                .ThenInclude(cl => cl.Product) 
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
                        .ThenInclude(item => item.Product)
                        .Include(o => o.CuttingLists)
                        .ThenInclude(cl => cl.Product)
                        .ToListAsync();

            return orders;
        }

    }
}
