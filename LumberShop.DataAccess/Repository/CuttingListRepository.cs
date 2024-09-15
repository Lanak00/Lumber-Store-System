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
    public class CuttingListRepository : ICuttingListRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public CuttingListRepository(LumberStoreSystemDbContext context)
        {
            _context = context;
        }

        public async Task Add(CuttingList cuttingList)
        {
             _context.CuttingLists.Add(cuttingList);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(CuttingList cuttingList)
        {
            _context.CuttingLists.Remove(cuttingList);
            await _context.SaveChangesAsync();
        }

        public async Task<CuttingList> GetById(int id)
        {
            return await _context.CuttingLists.FindAsync(id);
        }

        public async Task<IEnumerable<CuttingList>> GetAll()
        {
            return await _context.CuttingLists.ToListAsync();
        }

        public async Task<IEnumerable<CuttingList>> GetByOrderId(int orderId)
        {
            return await _context.CuttingLists
                         .Where(cl => cl.OrderId == orderId)
                         .ToListAsync();
        }
    }
}
