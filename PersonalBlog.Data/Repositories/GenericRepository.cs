using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : BaseModel
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TModel> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TModel>();
        }

        public async Task CreateAsync(TModel modelToCreate)
        {
            await _dbSet.AddAsync(modelToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(IEnumerable<TModel> modelsToDelete)
        {
             _dbSet.RemoveRange(modelsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TModel modelToDelete)
        {
            _dbSet.Remove(modelToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid modelId)
        {
            var model = await _dbSet.FirstOrDefaultAsync(m => m.Id == modelId);
             _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TModel> GetByIdAsync(Guid modelId)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Id == modelId);
        }

        public async Task UpdateAsync(TModel modelToUpdate)
        {
            _context.Entry(modelToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
