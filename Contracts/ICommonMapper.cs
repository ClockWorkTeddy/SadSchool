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

        public StartTimeViewModel StartTimeToVm(StartTime startTime);
        public StartTime StartTimeToModel(StartTimeViewModel startTimeViewModel);

        public ScheduledLessonViewModel ScheduledLessonToVm(ScheduledLesson scheduledLesson);
        public ScheduledLesson ScheduledLessonToModel(ScheduledLessonViewModel scheduledLessonViewModel);

        public LessonViewModel LessonToVm(Lesson lesson);
        public Lesson LessonToModel(LessonViewModel lessonViewModel);

        public ClassViewModel ClassToVm(Class @class);
        public Class ClassToModel(ClassViewModel classViewModel);
    }
}
