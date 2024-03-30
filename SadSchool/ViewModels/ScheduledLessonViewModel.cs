// <copyright file="ScheduledLessonViewModel.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// Represents the scheduled lesson view model.
    /// </summary>
    public class ScheduledLessonViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public string? Day { get; set; }

        /// <summary>
        /// Gets or sets the StartTime's id.
        /// </summary>
        [Required(ErrorMessage = "You have to choose a start time!")]
        public int? StartTimeId { get; set; }

        /// <summary>
        /// Gets or sets the StartTime's value.
        /// </summary>
        public string? StartTimeValue { get; set; }

        /// <summary>
        /// Gets or sets the list of StartTimes.
        /// </summary>
        public List<SelectListItem> StartTimes { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets the subject's id.
        /// </summary>
        [Required(ErrorMessage = "You have to choose a subject!")]
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the subject's name.
        /// </summary>
        public string? SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the list of subjects.
        /// </summary>
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets the class's id.
        /// </summary>
        [Required(ErrorMessage = "You have to choose a class!")]
        public int? ClassId { get; set; }

        /// <summary>
        /// Gets or sets the class's name.
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Gets or sets the list of classes.
        /// </summary>
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets the teacher's id.
        /// </summary>
        [Required(ErrorMessage = "You have to choose a teacher!")]
        public int? TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the teacher's name.
        /// </summary>
        public string? TeacherName { get; set; }

        /// <summary>
        /// Gets or sets the list of teachers.
        /// </summary>
        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
    }
}
