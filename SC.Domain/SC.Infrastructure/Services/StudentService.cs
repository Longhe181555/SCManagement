using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;
using SC.Infrastructure.Data;

namespace SC.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.Include(s => s.StudentEnrollments)
                .ThenInclude(e => e.Class).ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.Include(s => s.StudentEnrollments)
                .ThenInclude(e => e.Class).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetStudentByNameAsync(string name)
        {
            return await _context.Students.Include(s => s.StudentEnrollments)
                .ThenInclude(e => e.Class).FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                return false;
            }

            existingStudent.Name = student.Name;
            existingStudent.Address = student.Address;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.DateOfBirth = student.DateOfBirth;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
