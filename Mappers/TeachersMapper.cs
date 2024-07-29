namespace SadSchool.Mappers
{
    using Riok.Mapperly.Abstractions;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    [Mapper]
    public partial class TeacherMapper
    {
        public partial TeacherViewModel Map(Teacher teacher);
    }
}
