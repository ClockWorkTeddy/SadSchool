namespace SadSchool.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string Sex { get; set; } = null!;
        public string ClassName { get; set; } = null!;
    }
}
