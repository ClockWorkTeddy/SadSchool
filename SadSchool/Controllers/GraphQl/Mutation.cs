// <copyright file="Mutation.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.GraphQl
{
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// Mutation class.
    /// </summary>
    public class Mutation
    {
        /// <summary>
        /// Creates a new class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="name">Name of the created class.</param>
        /// <param name="teacherId">Id of the class's teacher.</param>
        /// <returns>New class instance.</returns>
        public Class CreateClass(SadSchoolContext context, string name, int teacherId)
        {
            var newClass = new Class()
            {
                Name = name,
                TeacherId = teacherId,
            };

            context.Classes.Add(newClass);
            context.SaveChanges();

            return newClass;
        }

        /// <summary>
        /// Creates a new lesson.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="date">Date of the lesson.</param>
        /// <param name="scheduledLessonId">Id of the related scheduled lesson.</param>
        /// <returns>A new instance of a lesson class.</returns>
        public Lesson CreateLesson(SadSchoolContext context, string date, int scheduledLessonId)
        {
            var newLesson = new Lesson()
            {
                Date = date,
                ScheduledLessonId = scheduledLessonId,
            };

            context.Lessons.Add(newLesson);
            context.SaveChanges();

            return newLesson;
        }

        /// <summary>
        /// Creates a new Scheduled Lesson.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="startTimeId">Start time Id of the lesson.</param>
        /// <param name="subjectId">Subject's id of the lesson.</param>
        /// <param name="classId">Id of the class of the lesson.</param>
        /// <param name="teacherId">If of the teacher.</param>
        /// <param name="day">Lesson's day.</param>
        /// <returns>A new ScheduledLesson instance.</returns>
        public ScheduledLesson CreateScheduledLesson(
            SadSchoolContext context,
            int startTimeId,
            int subjectId,
            int classId,
            int teacherId,
            string day)
        {
            var newScheduledLesson = new ScheduledLesson
            {
                StartTimeId = startTimeId,
                SubjectId = subjectId,
                ClassId = classId,
                TeacherId = teacherId,
                Day = day,
            };

            context.ScheduledLessons.Add(newScheduledLesson);
            context.SaveChangesAsync();

            return newScheduledLesson;
        }

        /// <summary>
        /// Creates a new start time.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="value">Start time's value.</param>
        /// <returns>A new StartTime instance.</returns>
        public StartTime CreateStartTime(SadSchoolContext context, string value)
        {
            var newStartTime = new StartTime
            {
                Value = value,
            };

            context.StartTimes.Add(newStartTime);
            context.SaveChangesAsync();

            return newStartTime;
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="firstName">First name of the student.</param>
        /// <param name="lastName">Last name of the student.</param>
        /// <param name="classId">Id of the class of the student.</param>
        /// <param name="dateOfBirth">Student's date of birth.</param>
        /// <returns>New student object.</returns>
        public Student CreateStudent(
            SadSchoolContext context,
            string firstName,
            string lastName,
            int classId,
            string dateOfBirth)
        {
            var newStudent = new Student()
            {
                FirstName = firstName,
                LastName = lastName,
                ClassId = classId,
                DateOfBirth = DateOnly.Parse(dateOfBirth, System.Globalization.CultureInfo.InvariantCulture),
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudent;
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="name">The name of the subject.</param>
        /// <returns>A new subject instance.</returns>
        public Subject CreateSubject(SadSchoolContext context, string name)
        {
            var newSubject = new Subject
            {
                Name = name,
            };

            context.Subjects.Add(newSubject);
            context.SaveChangesAsync();

            return newSubject;
        }

        /// <summary>
        /// Creates a new teacher.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="firstName">The first name of the teacher.</param>
        /// <param name="lastName">The last name of the teacher.</param>
        /// <param name="dateOfBirth">Teacher's date of birth.</param>
        /// <param name="grade">Teacher's grade.</param>
        /// <returns>A new Teacher instance.</returns>
        public Teacher CreateTeacher(
            SadSchoolContext context,
            string firstName,
            string lastName,
            string dateOfBirth,
            int grade)
        {
            var newTeacher = new Teacher
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = DateOnly.Parse(dateOfBirth, System.Globalization.CultureInfo.InvariantCulture),
                Grade = grade,
            };

            context.Teachers.Add(newTeacher);
            context.SaveChangesAsync();

            return newTeacher;
        }
    }
}
