
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;

namespace SC.Infrastructure.Services
{
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly IStudentEnrollmentRepository _studentEnrollmentRepository;

        public StudentEnrollmentService(IStudentEnrollmentRepository studentEnrollmentRepository)
        {
            _studentEnrollmentRepository = studentEnrollmentRepository;
        }

        public Task<StudentEnrollment?> GetEnrollmentByStudentIdAndClassIdAsync(int studentId, int classId)
        {
            return _studentEnrollmentRepository.GetEnrollmentByStudentIdAndClassIdAsync(studentId, classId);
        }

        public Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            return _studentEnrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
        }

        public Task<IEnumerable<StudentEnrollment>> GetEnrollmentsByClassIdAsync(int classId)
        {
            return _studentEnrollmentRepository.GetEnrollmentsByClassIdAsync(classId);
        }

        public Task AddEnrollmentAsync(StudentEnrollment enrollment)
        {
            return _studentEnrollmentRepository.AddEnrollmentAsync(enrollment);
        }

        public Task DeleteEnrollmentAsync(int studentId, int classId)
        {
            return _studentEnrollmentRepository.DeleteEnrollmentAsync(studentId, classId);
        }
    }
}
