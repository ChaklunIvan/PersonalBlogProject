using PersonalBlog.Data.Models.Base;

namespace PersonalBlog.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : BaseModel
    {
        Task CreateAsync(TModel modelToCreate);
        Task DeleteAsync(TModel modelToDelete);
        Task DeleteAllAsync(IEnumerable<TModel> modelsToDelete);
        Task DeleteByIdAsync(Guid modelId);
        Task UpdateAsync(TModel modelToUpdate);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid modelId);
    }
}
