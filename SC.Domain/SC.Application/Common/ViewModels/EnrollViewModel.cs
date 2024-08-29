

namespace SC.Application.Common.ViewModels
{
    public class EnrollViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<int> Sid { get; set; } = new List<int>();
    }
}
