// <copyright file="Mutation.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.GraphQl
{
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// Mutation class.
    /// </summary>
    public class Mutation
    {
        /// <summary>
        /// Creates a new class.
        /// </summary>
        /// <param name="classRepository">Class repo instance.</param>
        /// <param name="name">Name of the created class.</param>
        /// <param name="teacherId">Id of the class's teacher.</param>
        /// <returns>New class instance.</returns>
        public async Task<Class> CreateClass([Service] IClassRepository classRepository, string name, int teacherId)
        {
            var newClass = new Class()
            {
                Name = name,
                TeacherId = teacherId,
            };

            await classRepository.AddEntityAsync(newClass);

            return newClass;
        }

        /// <summary>
        /// Creates a new lesson.
        /// </summary>
        /// <param name="lessonRepository">Lesson repo instance.</param>
        /// <param name="date">Date of the lesson.</param>
        /// <param name="scheduledLessonId">Id of the related scheduled lesson.</param>
        /// <returns>A new instance of a lesson class.</returns>
        public async Task<Lesson> CreateLesson([Service] ILessonRepository lessonRepository, string date, int scheduledLessonId)
        {
            var newLesson = new Lesson()
            {
                Date = date,
                ScheduledLessonId = scheduledLessonId,
            };

            await lessonRepository.AddEntityAsync(newLesson);

            return newLesson;
        }

        /// <summary>
        /// Creates a new Scheduled Lesson.
        /// </summary>
        /// <param name="scheduledLessonRepository">Scheduled lesson repo instance.</param>
        /// <param name="startTimeId">Start time Id of the lesson.</param>
        /// <param name="subjectId">Subject's id of the lesson.</param>
        /// <param name="classId">Id of the class of the lesson.</param>
        /// <param name="teacherId">If of the teacher.</param>
        /// <param name="day">Lesson's day.</param>
        /// <returns>A new ScheduledLesson instance.</returns>
        public async Task<ScheduledLesson> CreateScheduledLesson(
            [Service] IScheduledLessonRepository scheduledLessonRepository,
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

            await scheduledLessonRepository.AddEntityAsync(newScheduledLesson);

            return newScheduledLesson;
        }

        /// <summary>
        /// Creates a new start time.
        /// </summary>
        /// <param name="startTimeRepository">Start time repo instance.</param>
        /// <param name="value">Start time's value.</param>
        /// <returns>A new StartTime instance.</returns>
        public async Task<StartTime> CreateStartTime([Service] IStartTimeRepository startTimeRepository, string value)
        {
            var newStartTime = new StartTime
            {
                Value = value,
            };

            await startTimeRepository.AddEntityAsync(newStartTime);

            return newStartTime;
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="studentRepository">Student repo instance.</param>
        /// <param name="firstName">First name of the student.</param>
        /// <param name="lastName">Last name of the student.</param>
        /// <param name="classId">Id of the class of the student.</param>
        /// <param name="dateOfBirth">Student's date of birth.</param>
        /// <returns>New student object.</returns>
        public async Task<Student> CreateStudent(
            [Service] IStudentRepository studentRepository,
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

            await studentRepository.AddEntityAsync(newStudent);
            return newStudent;
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="subjectRepository">Subject repo instance.</param>
        /// <param name="name">The name of the subject.</param>
        /// <returns>A new subject instance.</returns>
        public async Task<Subject> CreateSubject([Service] ISubjectRepository subjectRepository, string name)
        {
            var newSubject = new Subject
            {
                Name = name,
            };

            await subjectRepository.AddEntityAsync(newSubject);

            return newSubject;
        }

        /// <summary>
        /// Creates a new teacher.
        /// </summary>
        /// <param name="teacherRepository">Teacher repo instance.</param>
        /// <param name="firstName">The first name of the teacher.</param>
        /// <param name="lastName">The last name of the teacher.</param>
        /// <param name="dateOfBirth">Teacher's date of birth.</param>
        /// <param name="grade">Teacher's grade.</param>
        /// <returns>A new Teacher instance.</returns>
        public async Task<Teacher> CreateTeacher(
            [Service] ITeacherRepository teacherRepository,
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

            await teacherRepository.AddEntityAsync(newTeacher);

            return newTeacher;
        }
    }
}
