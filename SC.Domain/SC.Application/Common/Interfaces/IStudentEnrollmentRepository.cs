using SC.Domain.Entities;


namespace SC.Application.Common.Interfaces
{
    public interface IStudentEnrollmentRepository
    {
        Task<StudentEnrollment?> GetEnrollmentByStudentIdAndClassIdAsync(int studentId, int classId);
        Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByClassIdAsync(int classId);
        Task AddEnrollmentAsync(StudentEnrollment enrollment);
        Task DeleteEnrollmentAsync(int studentId, int classId);
    }
}
