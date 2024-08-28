using System.Collections.Specialized;

namespace SC.Domain.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public IList<StudentEnrollment> StudentEnrollments { get; set; } = new List<StudentEnrollment>();
    }
}
