namespace SadSchool.Mappers
{
    using Riok.Mapperly.Abstractions;
    using SadSchool.Contracts;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    [Mapper]
    public partial class CommonMapper : ICommonMapper
    {
        public partial TeacherViewModel TeacherToVm(Teacher teacher);
        public partial Teacher TeacherToModel(TeacherViewModel teacherViewModel);

        public partial SubjectViewModel SubjectToVm(Subject subject);
        public partial Subject SubjectToModel(SubjectViewModel subjectViewModel);

        public partial StudentViewModel StudentToVm(Student student);
        public partial Student StudentToModel(StudentViewModel studentViewModel);
    }
}
