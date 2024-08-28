using SC.Application.Common.ViewModels;
using SC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.Application.Common.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class?> GetClassByIdAsync(int id);
        Task<Class?> GetClassByNameAsync(string name);
        Task<Class> CreateClassAsync(EnrollViewModel request);
        Task<Class> UpdateClassAsync(EnrollViewModel request);
        Task<bool> DeleteClassAsync(int id);
    }
}
