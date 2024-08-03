using SadSchool.Models.SqlServer;
using SadSchool.ViewModels;

namespace SadSchool.Contracts
{
    public interface ICommonMapper
    {
        public TeacherViewModel TeacherToVm(Teacher teacher);
        public Teacher TeacherToModel(TeacherViewModel teacherViewModel);

        public SubjectViewModel SubjectToVm(Subject subject);
        public Subject SubjectToModel(SubjectViewModel subjectViewModel);

        public StudentViewModel StudentToVm(Student student);
        public Student StudentToModel(StudentViewModel studentViewModel);
    }
}
