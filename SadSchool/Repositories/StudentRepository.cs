namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// Provides methods for accessing and querying student data from the database.
    /// </summary>
    /// <remarks>The <see cref="StudentRepository"/> class offers various methods to retrieve student
    /// information based on different criteria such as first name, last name, date of birth, class ID, and sex. It
    /// utilizes the underlying database context and caching service to efficiently manage data access.</remarks>
    /// <param name="context">DB context instance.</param>
    /// <param name="cacheService">Cache service instance.</param>
    public class StudentRepository(SadSchoolContext context, ICacheService cacheService)
        : BaseRepository(context, cacheService), IStudentRepository
    {
        /// <inheritdoc/>
        public async Task<List<Student>> GetStudentsByFirstNameAsync(string firstName)
        {
            return await this.context.Students.Where(s => s.FirstName == firstName).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Student>> GetStudentsByLastNameAsync(string lastName)
        {
            return await this.context.Students.Where(s => s.LastName == lastName).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Student>> GetStudentsByDateOfBirthAsync(DateOnly dateOfBirth)
        {
            return await this.context.Students.Where(s => s.DateOfBirth == dateOfBirth).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Student>> GetStudentsByClassIdAsync(int classId)
        {
            return await this.context.Students.Where(s => s.ClassId == classId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Student>> GetStudentsBySexAsync(bool isMale)
        {
            return await this.context.Students.Where(s => s.Sex == isMale).ToListAsync();
        }
    }
}
