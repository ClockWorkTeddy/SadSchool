// <copyright file="Mark.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models.Mongo
{
    using MongoDB.Bson;

    /// <summary>
    /// Mark model.
    /// </summary>
    public class Mark
    {
        /// <summary>
        /// Gets or sets objectId of the mark.
        /// </summary>
        public ObjectId Id { get; set; }

        public string IdString => Id.ToString();

        /// <summary>
        /// Gets or sets mark value (from 1 to 5, 5 is the best).
        /// </summary>
        public string? Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lesson id during which the mark has been received.
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// Gets or sets the student id who has received the mark.
        /// </summary>
        public int? StudentId { get; set; }
    }
}
