// <copyright file="AverageMarkModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Dtos
{
    /// <summary>
    /// Average mark model.
    /// </summary>
    public class AverageMarkDto
    {
        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string? SubjectName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mark value.
        /// </summary>
        public double MarkValue { get; set; }

        /// <summary>
        /// Overridden ToString().
        /// </summary>
        /// <returns>Returns the base.ToString() in case the MarkValue isn't a number,
        ///     and formatted value otherwise.</returns>
        public override string ToString()
        {
            if (this.MarkValue == double.NaN)
            {
                return base.ToString() ?? string.Empty;
            }
            else
            {
                return string.Format("{0:N2}", this.MarkValue);
            }
        }
    }
}
