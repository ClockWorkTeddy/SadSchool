
namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <inheritdoc/>
    public class TeacherRepository : BaseRepository, ITeacherRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="SadSchoolContext"/> instance used to interact with the database.  This parameter cannot be
        /// <see langword="null"/>.</param>
        /// <param name="cacheService">Cache service instance for caching operations.</param>
        public TeacherRepository(SadSchoolContext context, ICacheService cacheService)
            : base(context, cacheService)
        {
        }

        /// <inheritdoc/>
        public async Task<List<Teacher>> GetTeachersByFirstNameAsync(string firstName)
        {
            return await this.context.Teachers
                .Where(t => t.FirstName == firstName)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Teacher>> GetTeachersByLastNameAsync(string lastName)
        {
            return await this.context.Teachers
                .Where(t => t.LastName == lastName)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Teacher>> GetTeachersByDateOfBirthAsync(DateOnly dateOfBirth)
        {
            return await this.context.Teachers
                .Where(t => t.DateOfBirth == dateOfBirth)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Teacher>> GetTeachersByGradeAsync(int grade)
        {
            return await this.context.Teachers
                .Where(t => t.Grade == grade)
                .ToListAsync();
        }
    }
}
