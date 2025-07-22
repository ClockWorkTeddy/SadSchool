using SadSchool.Contracts.Repositories;

namespace SadSchool.Repositories
{
    /// <inheritdoc/>
    public class DerivedRepositories(
        IClassRepository classRepository,
        ILessonRepository lessonRepository,
        IMarkRepository markRepository,
        IScheduledLessonRepository scheduledLessonRepository,
        IStudentRepository studentRepository) : IDerivedRepositories
    {
        /// <inheritdoc/>
        public IClassRepository ClassRepository { get; } = classRepository;

        /// <inheritdoc/>
        public ILessonRepository LessonRepository { get; } = lessonRepository;

        /// <inheritdoc/>
        public IMarkRepository MarkRepository { get; } = markRepository;

        /// <inheritdoc/>
        public IScheduledLessonRepository ScheduledLessonRepository { get; } = scheduledLessonRepository;

        /// <inheritdoc/>
        public IStudentRepository StudentRepository { get; } = studentRepository;
    }
}
