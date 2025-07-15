namespace SadSchool.Repositories
{
    using SadSchool.Contracts.Repositories;

    public class Repositories : IRepositories
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repositories"/> class with the specified repository implementations.
        /// </summary>
        /// <param name="classRepository">Class repo instance.</param>
        /// <param name="lessonRepository">Lesson repo instance.</param>
        /// <param name="scheduledLessonRepository">Scheduled lesson repo instance.</param>
        /// <param name="teacherRepository">Teacher repo instance.</param>
        /// <param name="startTimeRepository">Start time repo instance.</param>
        /// <param name="subjectRepository">Subject repo instance.</param>
        /// <param name="studentRepository">Student repo instance.</param>
        /// <param name="markRepository">Mark repo instance.</param>
        public Repositories(
            IClassRepository classRepository,
            ILessonRepository lessonRepository,
            IScheduledLessonRepository scheduledLessonRepository,
            ITeacherRepository teacherRepository,
            IStartTimeRepository startTimeRepository,
            ISubjectRepository subjectRepository,
            IStudentRepository studentRepository,
            IMarkRepository markRepository)
        {
            this.ClassRepository = classRepository;
            this.LessonRepository = lessonRepository;
            this.ScheduledLessonRepository = scheduledLessonRepository;
            this.TeacherRepository = teacherRepository;
            this.StartTimeRepository = startTimeRepository;
            this.SubjectRepository = subjectRepository;
            this.StudentRepository = studentRepository;
            this.MarkRepository = markRepository;
        }

        /// <inheritdoc/>
        public IClassRepository ClassRepository { get; }

        /// <inheritdoc/>
        public ILessonRepository LessonRepository { get; }

        /// <inheritdoc/>
        public IScheduledLessonRepository ScheduledLessonRepository { get; }

        /// <inheritdoc/>
        public ITeacherRepository TeacherRepository { get; }

        /// <inheritdoc/>
        public IStartTimeRepository StartTimeRepository { get; }

        /// <inheritdoc/>
        public ISubjectRepository SubjectRepository { get; }

        /// <inheritdoc/>
        public IStudentRepository StudentRepository { get; }

        /// <inheritdoc/>
        public IMarkRepository MarkRepository { get; }
    }
}
