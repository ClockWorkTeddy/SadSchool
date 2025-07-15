namespace SadSchool.Contracts.Repositories
{
    public interface IRepositories
    {
        /// <summary>
        /// A repository for managing teacher-related data operations.
        /// </summary>
        ITeacherRepository TeacherRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing start time data.
        /// </summary>
        IStartTimeRepository StartTimeRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing subject data.
        /// </summary>
        ISubjectRepository SubjectRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing class-related data.
        /// </summary>
        IClassRepository ClassRepository { get; }

        /// <summary>
        /// Gets the repository for accessing scheduled lesson data.
        /// </summary>
        IScheduledLessonRepository ScheduledLessonRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing student data.
        /// </summary>
        IStudentRepository StudentRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing lesson data.
        /// </summary>
        ILessonRepository LessonRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing and managing marks.
        /// </summary>
        IMarkRepository MarkRepository { get; }
    }
}
