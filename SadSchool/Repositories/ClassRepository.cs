// <copyright file="ClassRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using Serilog;

    /// <inheritdoc/>
    public class ClassRepository : IClassRepository
    {
        private readonly SadSchoolContext context;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRepository"/> class.
        /// </summary>
        /// <param name="context">SadSchoolDbContext.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public ClassRepository(SadSchoolContext context, ICacheService cacheService)
        {
            this.context = context;
            this.cacheService = cacheService;
        }

        /// <inheritdoc/>
        public async Task<List<Class>> GetAllClassesAsync()
        {
            var classes = this.cacheService.GetObjects<Class>();

            if (classes != null)
            {
                return classes;
            }

            classes = await this.context.Classes.ToListAsync();

            this.cacheService.SetObjects(classes);

            return classes;
        }

        /// <inheritdoc/>
        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var @class = this.cacheService.GetObject<Class>(id);

            if (@class != null)
            {
                return @class;
            }

            @class = await this.context.Classes.FindAsync(id);

            if (@class != null)
            {
                this.cacheService.SetObject(@class);
            }

            return @class;
        }

        /// <inheritdoc/>
        public async Task<Class?> AddClassAsync(Class theClass)
        {
            try
            {
                await this.context.Classes.AddAsync(theClass);
                await this.context.SaveChangesAsync();

                this.cacheService.RemoveObjects<Class>();
                this.cacheService.SetObject(theClass);

                return theClass;
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception as needed (e.g., log it, rethrow it, etc.)
                // For now, we just return null to indicate failure.
                Log.Error(ex, "Error adding lesson to the database.");

                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateClassAsync(Class theClass)
        {
            var existing = await this.context.Classes.FindAsync(theClass.Id);

            if (existing == null)
            {
                return false;
            }

            this.context.Entry(existing).CurrentValues.SetValues(theClass);

            await this.context.SaveChangesAsync();

            this.cacheService.RemoveObject(existing);
            this.cacheService.RemoveObjects<Class>();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteClassByIdAsync(int id)
        {
            var theClass = await this.GetClassByIdAsync(id);

            if (theClass != null)
            {
                this.context.Classes.Remove(theClass);
                await this.context.SaveChangesAsync();

                this.cacheService.RemoveObject(theClass);
                this.cacheService.RemoveObjects<Class>();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
