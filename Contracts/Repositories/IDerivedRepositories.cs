using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadSchool.Contracts.Repositories
{
    public interface IDerivedRepositories
    {
        /// <summary>
        /// Gets the repository for accessing class-related data.
        /// </summary>
        IClassRepository ClassRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing lesson data.
        /// </summary>
        ILessonRepository LessonRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing and managing marks.
        /// </summary>
        IMarkRepository MarkRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing scheduled lesson data.
        /// </summary>
        IScheduledLessonRepository ScheduledLessonRepository { get; }

        /// <summary>
        /// Gets the repository interface for accessing student data.
        /// </summary>
        IStudentRepository StudentRepository { get; }
    }
}
