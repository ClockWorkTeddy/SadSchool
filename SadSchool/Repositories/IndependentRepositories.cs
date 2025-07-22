using SadSchool.Contracts.Repositories;

namespace SadSchool.Repositories
{
    /// <inheritdoc/>
    public class IndependentRepositories(
        ITeacherRepository teacherRepository,
        IStartTimeRepository startTimeRepository,
        ISubjectRepository subjectRepository) : IIndependentRepositories
    {
        /// <inheritdoc/>
        public IStartTimeRepository StartTimeRepository { get; } = startTimeRepository;

        /// <inheritdoc/>
        public ISubjectRepository SubjectRepository { get; } = subjectRepository;

        /// <inheritdoc/>
        public ITeacherRepository TeacherRepository { get; } = teacherRepository;
    }
}
