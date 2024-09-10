using SadSchool.Models.SqlServer;
using SadSchool.ViewModels;

namespace SadSchool.Contracts
{
    public interface IScheduledLessonMapper
    {
        public ScheduledLessonViewModel ScheduledLessonToVm(ScheduledLesson scheduledLesson);
        public ScheduledLesson ScheduledLessonToModel(ScheduledLessonViewModel scheduledLessonViewModel);
    }
}
