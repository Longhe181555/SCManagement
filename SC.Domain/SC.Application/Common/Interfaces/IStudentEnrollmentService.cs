using SC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.Application.Common.Interfaces
{
    public interface IStudentEnrollmentService
    {
        Task<StudentEnrollment?> GetEnrollmentByStudentIdAndClassIdAsync(int studentId, int classId);
        Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByClassIdAsync(int classId);
        Task AddEnrollmentAsync(StudentEnrollment enrollment);
        Task DeleteEnrollmentAsync(int studentId, int classId);
    }
}
