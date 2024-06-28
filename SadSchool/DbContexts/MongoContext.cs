// <copyright file="MongoContext.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using Models.Mongo;
    using MongoDB.Driver;
    using MongoDB.EntityFrameworkCore.Extensions;

    /// <summary>
    /// Mongo context.
    /// </summary>
    public class MongoContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext"/> class.
        /// </summary>
        /// <param name="options">Options for DB context creation.</param>
        public MongoContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or inits the marks collection.
        /// </summary>
        public DbSet<Mark> Marks { get; init; }

        /// <summary>
        /// Creates the Mongo context.
        /// </summary>
        /// <param name="database"><see cref="IMongoDatabase"/>Mongo DB instance.</param>
        /// <returns>MongoContext Instance.</returns>
        public static MongoContext Create(IMongoDatabase database)
        {
            return new MongoContext(new DbContextOptionsBuilder<MongoContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);
        }

        /// <summary>
        /// This method sets up model relations.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>Model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mark>().ToCollection("marks");
        }
    }
}
