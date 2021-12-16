using PersonalBlog.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : BaseModel
    {
        Task CreateAsync(TModel modelToCreate);
        Task DeleteAsync(TModel modelToDelete);
        Task DeleteByIdAsync(Guid modelId);
        Task UpdateAsync(TModel modelToUpdate);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid modelId);
    }
}
