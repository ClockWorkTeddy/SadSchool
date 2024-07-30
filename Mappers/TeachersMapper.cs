namespace SadSchool.Mappers
{
    using Riok.Mapperly.Abstractions;
    using SadSchool.Contracts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    [Mapper]
    public partial class TeacherMapper : ITeacherMapper
    {
        public partial TeacherViewModel ToViewModel(Teacher teacher);
        public partial Teacher ToModel(TeacherViewModel teacherViewModel);
    }
}
