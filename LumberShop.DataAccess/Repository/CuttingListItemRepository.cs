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
    public class CuttingListItemRepository : ICuttingListItemRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public CuttingListItemRepository(LumberStoreSystemDbContext context)
        {
            _context = context;
        }
        public async Task Add(CuttingListItem cuttingListItem)
        {
            _context.CuttingListItems.Add(cuttingListItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(CuttingListItem CuttingListItems)
        {
            _context.CuttingListItems.Remove(CuttingListItems);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CuttingListItem>> GetByCuttingListId(int cuttingListId)
        {
            return await _context.CuttingListItems
                        .Where(cl => cl.CuttingListId == cuttingListId)
                        .ToListAsync();
        }

        public async Task<CuttingListItem> GetById(int id)
        {
            return await _context.CuttingListItems.FindAsync(id);
        }

        public async Task Update(CuttingListItem cuttingListItem)
        {
            _context.CuttingListItems.Update(cuttingListItem);
            await _context.SaveChangesAsync();
        }
    }
}
