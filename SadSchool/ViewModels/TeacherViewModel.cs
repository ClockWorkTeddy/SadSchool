namespace SadSchool.ViewModels
{
    public class TeacherViewModel
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public int? Grade { get; set; }
    }
}
