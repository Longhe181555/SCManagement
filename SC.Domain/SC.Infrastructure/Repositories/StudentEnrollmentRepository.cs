using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;
using SC.Infrastructure.Data;

namespace SC.Infrastructure.Repositories
{
    public class StudentEnrollmentRepository : IStudentEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentEnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentEnrollment?> GetEnrollmentByStudentIdAndClassIdAsync(int studentId, int classId)
        {
            return await _context.StudentEnrollments
                .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ClassId == classId);
        }

        public async Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            return await _context.StudentEnrollments
                .Include(se => se.Class) 
                .Where(se => se.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByClassIdAsync(int classId)
        {
            return await _context.StudentEnrollments
                .Include(se => se.Student) 
                .Where(se => se.ClassId == classId)
                .ToListAsync();
        }

        public async Task AddEnrollmentAsync(StudentEnrollment enrollment)
        {
            _context.StudentEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollmentAsync(int studentId, int classId)
        {
            var enrollment = await _context.StudentEnrollments
                .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ClassId == classId);
            if (enrollment != null)
            {
                _context.StudentEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
