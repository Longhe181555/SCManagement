namespace SC.Domain.Entities
{
    public class StudentEnrollment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } = new Student();
        public int ClassId { get; set; }
        public Class Class { get; set; } = new Class();
    }
}
