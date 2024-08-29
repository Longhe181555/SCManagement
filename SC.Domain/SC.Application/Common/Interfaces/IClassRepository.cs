using SC.Domain.Entities;

namespace SC.Application.Common.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task<Class?> GetByNameAsync(string name);
        Task AddAsync(Class newClass);
        Task UpdateAsync(Class updatedClass);
        Task DeleteAsync(int id);
    }
}
