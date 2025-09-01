// <copyright file="Query.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

#pragma warning disable S2325 // GraphQL won't work with static methods

namespace SadSchool.Controllers.GraphQl
{
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// GraphQL query class for obtaining the data.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Gets a class instance by id.
        /// </summary>
        /// <param name="classRepository">Class repo instance.</param>
        /// <param name="id">Id of the desirable class.</param>
        /// <returns>Class instance.</returns>
        public async Task<Class?> GetClass([Service] IClassRepository classRepository, int id)
        {
            return await classRepository.GetEntityByIdAsync<Class>(id);
        }

        /// <summary>
        /// Gets all class instances.
        /// </summary>
        /// <param name="classRepository">Class repo instance.</param>
        /// <returns>List of class instances.</returns>
        public async Task<IEnumerable<Class>> GetClasses([Service] IClassRepository classRepository)
        {
            return await classRepository.GetAllEntitiesAsync<Class>();
        }

        /// <summary>
        /// Gets a lesson by id.
        /// </summary>
        /// <param name="lessonRepository">Lesson repo instance.</param>
        /// <param name="id">Id of the desirable lesson.</param>
        /// <returns>Lesson instance.</returns>
        public async Task<Lesson?> GetLesson([Service] ILessonRepository lessonRepository, int id)
        {
            return await lessonRepository.GetEntityByIdAsync<Lesson>(id);
        }

        /// <summary>
        /// Gets all lesson instances.
        /// </summary>
        /// <param name="lessonRepository">Lesson repo instance.</param>
        /// <returns>List of lesson instances.</returns>
        public async Task<IEnumerable<Lesson>> GetLessons([Service] ILessonRepository lessonRepository)
        {
            return await lessonRepository.GetAllEntitiesAsync<Lesson>();
        }

        /// <summary>
        /// Gets a student by id.
        /// </summary>
        /// <param name="studentRepository">Student repo instance.</param>
        /// <param name="id">Id of the desirable student.</param>
        /// <returns>Student instance.</returns>
        public async Task<Student?> GetStudent([Service] IStudentRepository studentRepository, int id)
        {
            return await studentRepository.GetEntityByIdAsync<Student>(id);
        }

        /// <summary>
        /// Gets all student instances.
        /// </summary>
        /// <param name="studentRepository">Student repo instance.</param>
        /// <returns>List of student instances.</returns>
        public async Task<IEnumerable<Student>> GetStudents([Service] IStudentRepository studentRepository)
        {
            return await studentRepository.GetAllEntitiesAsync<Student>();
        }

        /// <summary>
        /// Gets a teacher by id.
        /// </summary>
        /// <param name="teacherRepository">Teacher repo instance.</param>
        /// <param name="id">Id of the desirable teacher.</param>
        /// <returns>Teacher instance.</returns>
        public async Task<Teacher?> GetTeacher([Service] ITeacherRepository teacherRepository, int id)
        {
            return await teacherRepository.GetEntityByIdAsync<Teacher>(id);
        }

        /// <summary>
        /// Gets all teachers instances.
        /// </summary>
        /// <param name="teacherRepository">Teacher repo instance.</param>
        /// <returns>List of teacher instances.</returns>
        public async Task<IEnumerable<Teacher>> GetTeachers([Service] ITeacherRepository teacherRepository)
        {
            return await teacherRepository.GetAllEntitiesAsync<Teacher>();
        }

        /// <summary>
        /// Gets a subject by id.
        /// </summary>
        /// <param name="subjectRepository">Subject repo instance.</param>
        /// <param name="id">Id of the desirable subject.</param>
        /// <returns>Subject instance.</returns>
        public async Task<Subject?> GetSubject([Service] ISubjectRepository subjectRepository, int id)
        {
            return await subjectRepository.GetEntityByIdAsync<Subject>(id);
        }

        /// <summary>
        /// Gets all subject instances.
        /// </summary>
        /// <param name="subjectRepository">Subject repo instance.</param>
        /// <returns>List of subject instances.</returns>
        public async Task<IEnumerable<Subject>> GetSubjects([Service] ISubjectRepository subjectRepository)
        {
            return await subjectRepository.GetAllEntitiesAsync<Subject>();
        }

        /// <summary>
        /// Gets a scheduled lesson by id.
        /// </summary>
        /// <param name="scheduledLessonRepository">Scheduled lesson repo instance.</param>
        /// <param name="id">Id of the desirable scheduled lesson.</param>
        /// <returns>Lesson instance.</returns>
        public async Task<ScheduledLesson?> GetScheduledLesson([Service] IScheduledLessonRepository scheduledLessonRepository, int id)
        {
            return await scheduledLessonRepository.GetEntityByIdAsync<ScheduledLesson>(id);
        }

        /// <summary>
        /// Gets all cheduled lesson instances.
        /// </summary>
        /// <param name="scheduledLessonRepository">Scheduled lesson repo instance.</param>
        /// <returns>List of cheduled lesson instances.</returns>
        public async Task<IEnumerable<ScheduledLesson>> GetScheduledLessons([Service] IScheduledLessonRepository scheduledLessonRepository)
        {
            return await scheduledLessonRepository.GetAllEntitiesAsync<ScheduledLesson>();
        }

        /// <summary>
        /// Gets a start time by id.
        /// </summary>
        /// <param name="startTimeRepository">Start time repo instance.</param>
        /// <param name="id">Id of the desirable start time.</param>
        /// <returns>Start time instance.</returns>
        public async Task<StartTime?> GetStartTime([Service] IStartTimeRepository startTimeRepository, int id)
        {
            return await startTimeRepository.GetEntityByIdAsync<StartTime>(id);
        }

        /// <summary>
        /// Gets all start time instances.
        /// </summary>
        /// <param name="startTimeRepository">Start time repo instance.</param>
        /// <returns>List of start time instances.</returns>
        public async Task<IEnumerable<StartTime>> GetStartTimes([Service] IStartTimeRepository startTimeRepository)
        {
            return await startTimeRepository.GetAllEntitiesAsync<StartTime>();
        }
    }
}

#pragma warning restore S2325 // GraphQL won't work with static methods
