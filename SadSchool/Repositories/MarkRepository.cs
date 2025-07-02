// <copyright file="MarkRepository.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;
    using SadSchool.Models.Mongo;

    /// <summary>
    /// Provides methods for managing and retrieving <see cref="Mark"/> entities in the data store.
    /// </summary>
    /// <remarks>This repository offers functionality to perform CRUD operations on <see cref="Mark"/>
    /// entities, including retrieving marks by student or lesson identifiers, adding new marks, updating existing
    /// marks, and deleting marks by their unique identifier. It interacts with the underlying data store through the
    /// provided <see cref="MongoContext"/>.</remarks>
    public class MarkRepository : IMarkRepository
    {
        private readonly MongoContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The <see cref="MongoContext"/> used to interact with the database. Cannot be null.</param>
        public MarkRepository(MongoContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Asynchronously retrieves a list of marks associated with the specified student ID.
        /// </summary>
        /// <remarks>This method queries the underlying data source to retrieve marks for the specified
        /// student.  Ensure that the <paramref name="studentId"/> is valid and corresponds to an existing
        /// student.</remarks>
        /// <param name="studentId">The unique identifier of the student whose marks are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mark"/>
        /// objects associated with the specified student ID. If no marks are found, an empty list is returned.</returns>
        public async Task<List<Mark>> GetMarksByStudentIdAsync(int studentId)
        {
            var filter = Builders<Mark>.Filter.Eq(m => m.StudentId, studentId);

            return await this.context.Marks.Where(m => m.StudentId == studentId).ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of marks for a specific student and lesson.
        /// </summary>
        /// <remarks>This method queries the data source to find all marks that match the specified
        /// student and lesson identifiers.</remarks>
        /// <param name="studentId">The unique identifier of the student whose marks are to be retrieved.</param>
        /// <param name="lessonId">The unique identifier of the lesson for which marks are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mark"/>
        /// objects associated with the specified student and lesson. If no marks are found, the list will be empty.</returns>
        public async Task<List<Mark>> GetMarksByStudentIdAndLessonIdAsync(int studentId, int lessonId)
        {
            var filter = Builders<Mark>.Filter.And(
                Builders<Mark>.Filter.Eq(m => m.StudentId, studentId),
                Builders<Mark>.Filter.Eq(m => m.LessonId, lessonId));

            return await this.context.Marks.Where(m => m.StudentId == studentId && m.LessonId == lessonId).ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all marks from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mark"/>
        /// objects representing all marks in the database.</returns>
        public async Task<List<Mark>> GetAllMarksAsync()
        {
            return await this.context.Marks.ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a mark by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the mark to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the  <see cref="Mark"/> object
        /// if found; otherwise, <see langword="null"/>.</returns>
        public async Task<Mark?> GetMarkByIdAsync(ObjectId id)
        {
            return await this.context.Marks.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously adds a new mark to the database.
        /// </summary>
        /// <remarks>This method adds the specified <see cref="Mark"/> to the database context and saves
        /// the changes asynchronously. Ensure that the <paramref name="mark"/> object is properly initialized before
        /// calling this method.</remarks>
        /// <param name="mark">The <see cref="Mark"/> object to add. This parameter cannot be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddMarkAsync(Mark mark)
        {
            this.context.Marks.Add(mark);

            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the specified mark in the database asynchronously.
        /// </summary>
        /// <param name="mark">The mark entity to update. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateMarkAsync(Mark mark)
        {
            this.context.Marks.Update(mark);

            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a mark from the database based on the specified identifier.
        /// </summary>
        /// <remarks>If a mark with the specified <paramref name="id"/> is not found, no action is
        /// taken.</remarks>
        /// <param name="id">The unique identifier of the mark to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteMarkByIdAsync(ObjectId id)
        {
            var mark = await this.context.Marks.FindAsync(id);

            if (mark != null)
            {
                this.context.Marks.Remove(mark);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
