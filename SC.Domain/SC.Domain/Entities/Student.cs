namespace SC.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public IList<StudentEnrollment> StudentEnrollments { get; set; } = new List<StudentEnrollment>();
    }
}
