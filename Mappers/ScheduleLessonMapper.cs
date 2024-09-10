namespace SadSchool.Mappers
{
    using SadSchool.Contracts;
    using Riok.Mapperly.Abstractions;
    using SadSchool.Models.SqlServer;
    using SadSchool.ViewModels;

    [Mapper]
    public partial class ScheduleLessonMapper : IScheduledLessonMapper
    {
        [MapProperty(nameof(ScheduledLesson.Teacher), nameof(ScheduledLessonViewModel.TeacherName))]
        public partial ScheduledLessonViewModel ScheduledLessonToVm(ScheduledLesson scheduledLesson);
        public partial ScheduledLesson ScheduledLessonToModel(ScheduledLessonViewModel scheduledLessonViewModel);
        
        private string MapTeacherName(Teacher teacher) =>
            $"{teacher.FirstName} {teacher.LastName}";
    }
}
