namespace SadSchool.ViewModels
{
    public class TeacherViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public int? Grade { get; set; }
    }
}
