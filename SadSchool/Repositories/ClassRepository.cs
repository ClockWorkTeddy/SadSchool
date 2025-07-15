// <copyright file="ClassRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <inheritdoc/>
    public class ClassRepository : BaseRepository, IClassRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRepository"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public ClassRepository(SadSchoolContext context, ICacheService cacheService)
            : base(context, cacheService)
        {
        }

        /// <inheritdoc/>
        public async Task<Class?> GetClassByNameAsync(string className)
        {
            var classes = await this.GetAllEntitiesAsync<Class>();
            if (classes == null || classes.Any())
            {
                return null;
            }

            return classes.FirstOrDefault(c => c != null && c.Name != null && c.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public async Task<List<Class>?> GetClassesByTeacherIdAsync(int teacherId)
        {
            var classes = await this.GetAllEntitiesAsync<Class>();
            if (classes == null || classes.Any())
            {
                return new List<Class>();
            }

            return classes.Where(c => c.TeacherId == teacherId).ToList();
        }
    }
}
