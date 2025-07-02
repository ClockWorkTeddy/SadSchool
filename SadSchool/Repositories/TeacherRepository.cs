
namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <inheritdoc/>
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="SadSchoolContext"/> instance used to interact with the database.  This parameter cannot be
        /// <see langword="null"/>.</param>
        public TeacherRepository(SadSchoolContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await this.context.Teachers.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await this.context.Teachers.FindAsync(id);
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

        /// <inheritdoc/>
        public async Task<Teacher?> AddTeacherAsync(Teacher teacher)
        {
            try
            {
                await this.context.Teachers.AddAsync(teacher);
                await this.context.SaveChangesAsync();

                return teacher;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateTeacherAsync(Teacher teacher)
        {
            var existing = await this.context.Teachers.FindAsync(teacher.Id);

            if (existing == null)
            {
                return false;
            }

            try
            {
                this.context.Entry(existing).CurrentValues.SetValues(teacher);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await this.context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return false;
            }

            try
            {
                this.context.Teachers.Remove(teacher);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
