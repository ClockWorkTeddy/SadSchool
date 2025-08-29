// <copyright file="IClassBookService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Contracts
{
    using SadSchool.ViewModels;

    /// <summary>
    /// The service for class books representation.
    /// </summary>
    public interface IClassBookService
    {
        /// <summary>
        /// Gets the row with dates of the mark those have been gotten by student.
        /// </summary>
        public List<string> Dates { get; }

        /// <summary>
        /// Gets the list of students in the particular class.
        /// </summary>
        public List<string> Students { get; }

        /// <summary>
        /// The method prepares students/dates table for using in the model.
        /// </summary>
        public Task GetMarkData();

        /// <summary>
        /// The mothod creates view model for class books.
        /// </summary>
        /// <param name="subjectName">Name of the selected subject.</param>
        /// <param name="className">Name of the selected class.</param>
        /// <returns>Class book view model with mark table of the particular subject and class.</returns>
        public Task<ClassBookViewModel> GetClassBookViewModel(string subjectName, string className);
    }
}
