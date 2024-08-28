using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;
using SC.Infrastructure.Data;
using System.Xml.Linq;

namespace SC.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes
                        .Include(c => c.StudentEnrollments)
                        .ThenInclude(se => se.Student)
                        .ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes
                        .Include(c => c.StudentEnrollments)
                        .ThenInclude(se => se.Student).FirstOrDefaultAsync(c => c.Id == id); ;
        }

        public async Task<Class?> GetByNameAsync(string name)
        {
            return await _context.Classes
                        .Include(c => c.StudentEnrollments)
                        .ThenInclude(se => se.Student).FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task AddAsync(Class newClass)
        {
            await _context.Classes.AddAsync(newClass);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class updatedClass)
        {
            _context.Classes.Update(updatedClass);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classToDelete = await _context.Classes.FindAsync(id);
            if (classToDelete != null)
            {
                _context.Classes.Remove(classToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
