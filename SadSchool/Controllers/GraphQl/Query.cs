// <copyright file="Query.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.GraphQl
{
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// GraphQL query class for obtaining the data.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Gets a class instance by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable class.</param>
        /// <returns>Class instance.</returns>
        public Class? GetClass(SadSchoolContext context, int id)
        {
            return context.Classes.Find(id);
        }

        /// <summary>
        /// Gets all class instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of class instances.</returns>
        public IEnumerable<Class> GetClasses(SadSchoolContext context)
        {
            return [.. context.Classes];
        }

        /// <summary>
        /// Gets a lesson by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable lesson.</param>
        /// <returns>Lesson instance.</returns>
        public Lesson? GetLesson(SadSchoolContext context, int id)
        {
            return context.Lessons.Find(id);
        }

        /// <summary>
        /// Gets all lesson instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of lesson instances.</returns>
        public IEnumerable<Lesson> GetLessons(SadSchoolContext context)
        {
            return context.Lessons.ToList();
        }

        /// <summary>
        /// Gets a student by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable student.</param>
        /// <returns>Student instance.</returns>
        public Student? GetStudent(SadSchoolContext context, int id)
        {
            return context.Students.Find(id);
        }

        /// <summary>
        /// Gets all student instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of student instances.</returns>
        public IEnumerable<Student> GetStudents(SadSchoolContext context)
        {
            return context.Students.ToList();
        }

        /// <summary>
        /// Gets a teacher by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable teacher.</param>
        /// <returns>Teacher instance.</returns>
        public Teacher? GetTeacher(SadSchoolContext context, int id)
        {
            return context.Teachers.Find(id);
        }

        /// <summary>
        /// Gets all teachers instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of teacher instances.</returns>
        public IEnumerable<Teacher> GetTeachers(SadSchoolContext context)
        {
            return context.Teachers.ToList();
        }

        /// <summary>
        /// Gets a subject by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable subject.</param>
        /// <returns>Subject instance.</returns>
        public Subject? GetSubject(SadSchoolContext context, int id)
        {
            return context.Subjects.Find(id);
        }

        /// <summary>
        /// Gets all subject instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of subject instances.</returns>
        public IEnumerable<Subject> GetSubjects(SadSchoolContext context)
        {
            return context.Subjects.ToList();
        }

        /// <summary>
        /// Gets a scheduled lesson by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable sceduled lesson.</param>
        /// <returns>Lesson instance.</returns>
        public ScheduledLesson? GetScheduledLesson(SadSchoolContext context, int id)
        {
            return context.ScheduledLessons.Find(id);
        }

        /// <summary>
        /// Gets all cheduled lesson instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of cheduled lesson instances.</returns>
        public IEnumerable<ScheduledLesson> GetScheduledLessons(SadSchoolContext context)
        {
            return context.ScheduledLessons.ToList();
        }

        /// <summary>
        /// Gets a start time by id.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <param name="id">Id of the desirable start time.</param>
        /// <returns>Start time instance.</returns>
        public StartTime? GetStartTime(SadSchoolContext context, int id)
        {
            return context.StartTimes.Find(id);
        }

        /// <summary>
        /// Gets all strt time instances.
        /// </summary>
        /// <param name="context">Db context.</param>
        /// <returns>List of start time instances.</returns>
        public IEnumerable<StartTime> GetStartTimes(SadSchoolContext context)
        {
            return context.StartTimes.ToList();
        }
    }
}
