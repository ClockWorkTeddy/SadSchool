
namespace SadSchool.Contracts.Repositories
{
    public interface IIndependentRepositories
    {
        /// <summary>
        /// Gets the repository interface for accessing and managing start time positions.
        /// </summary>
        IStartTimeRepository StartTimeRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing and managing subjects.
        /// </summary>
        ISubjectRepository SubjectRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing teacher data.
        /// </summary>
        ITeacherRepository TeacherRepository { get; }
    }
}
