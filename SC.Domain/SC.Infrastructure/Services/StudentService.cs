using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;
using SC.Infrastructure.Data;
using SC.Infrastructure.Repositories;

namespace SC.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentByIdAsync(id);
        }

        public async Task<Student?> GetStudentByNameAsync(string name)
        {
            return await _studentRepository.GetStudentByNameAsync(name);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            await _studentRepository.AddStudentAsync(student);
            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            if(student == null) { return false; }
            await _studentRepository.UpdateStudentAsync(student);
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
            return true;
        }
    }
}
