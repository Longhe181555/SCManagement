using SC.Domain.Entities;
using System;


namespace SC.Application.Common.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByNameAsync(string name);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
    }
}
