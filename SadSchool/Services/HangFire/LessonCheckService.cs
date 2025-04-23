using SadSchool.DbContexts;
using Serilog;

namespace SadSchool.Services.HangFire
{
    public class LessonCheckService
    {
        private readonly SadSchoolContext sadSchoolContext;

        public LessonCheckService(SadSchoolContext sadSchoolContext)
        {
            this.sadSchoolContext = sadSchoolContext;
        }

        public void DeleteLessonWithoutDate()
        {
            var lessonsWithoutDate = this.sadSchoolContext.Lessons
                .Where(x => x.Date == null)
                .ToList();

            if (lessonsWithoutDate.Count > 0)
            {
                var firstLessonWithoutDate = lessonsWithoutDate.First();
                this.sadSchoolContext.Remove(firstLessonWithoutDate);
                this.sadSchoolContext.SaveChanges();
                Log.Information($"LessonCheckService.DeleteLessonWithoutDate(): Deleted lesson without date, Id: {firstLessonWithoutDate.Id}");
            }
        }
    }
}
