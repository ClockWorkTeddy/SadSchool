// <copyright file="Mark.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Models
{
    /// <summary>
    /// Mark model.
    /// </summary>
    public class Mark : BaseModel
    {
        /// <summary>
        /// Gets or sets mark value (from 1 to 5, 5 is the best).
        /// </summary>
        public string? Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the lesson id during which the mark has been received.
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson object during which the mark has been got.
        /// </summary>
        public virtual Lesson? Lesson { get; set; }

        /// <summary>
        /// Gets or sets the student id who has received the mark.
        /// </summary>
        public int? StudentId { get; set; }

        /// <summary>
        /// Gets or sets the student object who has received the mark.
        /// </summary>
        public virtual Student? Student { get; set; }
    }
}
