using Microsoft.EntityFrameworkCore;
using SC.Application.Common.Interfaces;
using SC.Application.Common.ViewModels;
using SC.Domain.Entities;
using System;


namespace SC.Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentEnrollmentRepository _studentEnrollmentRepository;
        public ClassService(IClassRepository repository, IStudentEnrollmentRepository studentEnrollmentRepository, IStudentRepository studentRepository)
        {
            _classRepository = repository;
            _studentEnrollmentRepository = studentEnrollmentRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _classRepository.GetAllAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            return await _classRepository.GetByIdAsync(id);
        }

        public async Task<Class?> GetClassByNameAsync(string name)
        {
            return await _classRepository.GetByNameAsync(name);
        }

        public async Task<Class> CreateClassAsync(EnrollViewModel request)
        {
            var newClass = new Class
            {
                Name = request.Name,
                Address = request.Address
            };
            await _classRepository.AddAsync(newClass);

            var insertedClass = await _classRepository.GetByNameAsync(newClass.Name);

            foreach (var studentId in request.Sid)
            {
                    var student = await _studentRepository.GetStudentByIdAsync(studentId);
                    if (student != null)
                    {
                        var newEnrollment = new StudentEnrollment
                        {
                            StudentId = studentId,
                            Student = student,
                            ClassId = insertedClass!.Id,
                            Class = insertedClass
                        };
                        await _studentEnrollmentRepository.AddEnrollmentAsync(newEnrollment);
                    }
            }
            
            return newClass;
        }

        public async Task<Class> UpdateClassAsync(EnrollViewModel request)
        {
            var classToUpdate = await _classRepository.GetByIdAsync(request.Id);
            if (classToUpdate == null)
            {
                throw new KeyNotFoundException("Class not found");
            }

            classToUpdate.Name = request.Name;
            classToUpdate.Address = request.Address;

            var currentEnrollments = classToUpdate.StudentEnrollments.ToList();

            // Remove enrollments that are no longer in the request
            foreach (var enrollment in currentEnrollments)
            {
                if (!request.Sid.Contains(enrollment.StudentId))
                {
                    await _studentEnrollmentRepository.DeleteEnrollmentAsync(enrollment.StudentId, enrollment.ClassId);
                }
            }

            // Add new enrollments
            foreach (var studentId in request.Sid)
            {
                if (!currentEnrollments.Any(se => se.StudentId == studentId))
                {
                    var student = await _studentRepository.GetStudentByIdAsync(studentId);
                    if (student != null)
                    {
                        var newEnrollment = new StudentEnrollment
                        {
                            StudentId = studentId,
                            Student = student,
                            ClassId = classToUpdate.Id,
                            Class = classToUpdate
                        };
                        await _studentEnrollmentRepository.AddEnrollmentAsync(newEnrollment);
                    }
                }
            }
            await _classRepository.UpdateAsync(classToUpdate);
            return classToUpdate;
        }


        public async Task<bool> DeleteClassAsync(int id)
        {
            var classToDelete = await _classRepository.GetByIdAsync(id);
            if (classToDelete == null)
            {
                return false;
            }

            await _classRepository.DeleteAsync(id);
            return true;
        }
    }
}
