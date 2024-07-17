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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public OrderItemRepository(LumberStoreSystemDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(OrderItem orderItem)
        {
           _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderItem> GetById(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderId(int orderId)
        {
            return await _context.OrderItems
                        .Where(oi => oi.OrderId == orderId)
                        .ToListAsync();
        }

        public async Task Update(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
