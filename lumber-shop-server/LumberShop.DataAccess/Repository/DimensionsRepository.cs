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
    public class DimensionsRepository : IDimensionsRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public DimensionsRepository(LumberStoreSystemDbContext context)
        {
            _context = context;
        }
        public async Task<int> Add(Dimensions dimensions)
        {
            _context.Dimensions.Add(dimensions);
            await _context.SaveChangesAsync();
            return dimensions.Id;
        }

        public async Task Delete(Dimensions dimensions)
        {
            _context.Dimensions.Remove(dimensions);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Dimensions>> GetAll()
        {
            return await _context.Dimensions.ToListAsync();
        }

        public async Task<Dimensions> GetById(int id)
        {
            return await _context.Dimensions.FindAsync(id);
        }
        public async Task<int?> FindByAll(Dimensions dimensions)
        {
            var existingDimensions = await _context.Dimensions
             .FirstOrDefaultAsync(a => a.Height == dimensions.Height
                                  && a.Width == dimensions.Width
                                  && a.Length == dimensions.Length);
                              
            return existingDimensions?.Id;
        }
    }
}
