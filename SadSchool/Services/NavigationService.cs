// <copyright file="NavigationService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services
{
    using SadSchool.Contracts;
    using Serilog;

    /// <summary>
    /// Navigation service, pcoresses the "Back" button functions.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private Dictionary<string, UrlParams> map = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        public NavigationService()
        {
            this.map["Home:About"] = new UrlParams { Controller = "Home", Action = "Index" };
            this.map["Data:DataIndex"] = new UrlParams { Controller = "Home", Action = "Index" };
            this.map["Class:Classes"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Class:Add"] = new UrlParams { Controller = "Class", Action = "Classes" };
            this.map["Class:Edit"] = new UrlParams { Controller = "Class", Action = "Classes" };
            this.map["Lesson:Lessons"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Lesson:Add"] = new UrlParams { Controller = "Lesson", Action = "Lessons" };
            this.map["Lesson:Edit"] = new UrlParams { Controller = "Lesson", Action = "Lessons" };
            this.map["ScheduledLesson:ScheduledLessons"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["ScheduledLesson:Add"] = new UrlParams { Controller = "ScheduledLesson", Action = "ScheduledLessons" };
            this.map["ScheduledLesson:Edit"] = new UrlParams { Controller = "ScheduledLesson", Action = "ScheduledLessons" };
            this.map["Mark:Marks"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Mark:Add"] = new UrlParams { Controller = "Mark", Action = "Marks" };
            this.map["Mark:Edit"] = new UrlParams { Controller = "Mark", Action = "Marks" };
            this.map["StartTime:StartTimes"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["StartTime:Add"] = new UrlParams { Controller = "StartTime", Action = "StartTimes" };
            this.map["StartTime:Edit"] = new UrlParams { Controller = "StartTime", Action = "StartTimes" };
            this.map["Student:Students"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Student:Add"] = new UrlParams { Controller = "Student", Action = "Students" };
            this.map["Student:Edit"] = new UrlParams { Controller = "Student", Action = "Students" };
            this.map["Subject:Subjects"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Subject:Add"] = new UrlParams { Controller = "Subject", Action = "Subjects" };
            this.map["Subject:Edit"] = new UrlParams { Controller = "Subject", Action = "Subjects" };
            this.map["Teacher:Teachers"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Teacher:Add"] = new UrlParams { Controller = "Teacher", Action = "Teachers" };
            this.map["Teacher:Edit"] = new UrlParams { Controller = "Teacher", Action = "Teachers" };
            this.map["Schedule:GetSchedule"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["ClassBooks:ClassBooks"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["ClassBooks:ClassSelector"] = new UrlParams { Controller = "ClassBooks", Action = "ClassBooks" };
            this.map["ClassBooks:ClassBookTable"] = new UrlParams { Controller = "ClassBooks", Action = "ClassSelector" };
            this.map["Mark:GetStudentSubject"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            this.map["Mark:GetAverageMarks"] = new UrlParams { Controller = "Mark", Action = "GetStudentSubject" };
            this.map["Stuff:Stuff"] = new UrlParams { Controller = "Home", Action = "Index" };
            this.map["Users:Users"] = new UrlParams { Controller = "Stuff", Action = "Stuff" };
            this.map["Users:Register"] = new UrlParams { Controller = "Users", Action = "Users" };
            this.map["Users:RolesProcessing"] = new UrlParams { Controller = "Users", Action = "Users" };
            this.map["Users:AddRole"] = new UrlParams { Controller = "Users", Action = "RolesProcessing" };
        }

        /// <summary>
        /// Gets or sets the controller name.
        /// </summary>
        public string Controller { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets the class name.
        /// </summary>
        public string ClassName { get; private set; } = string.Empty;

        /// <summary>
        /// Stores the class name.
        /// </summary>
        /// <param name="className">Class name.</param>
        public void StoreClassName(string className)
        {
            Log.Information($"NavigationService.StoreClassName(): method called for className = {className}");

            this.ClassName = className;
        }

        /// <summary>
        /// Refreshes the "Back" button parameters.
        /// </summary>
        /// <param name="routeData">Controller and action names of the route.</param>
        public void RefreshBackParams(RouteData routeData)
        {
            Log.Information("NavigationService.RefreshBackParams(): method called.");

            string adress = this.GetAdress(routeData);

            this.Controller = this.map[adress].Controller;
            this.Action = this.map[adress].Action;
        }

        private string GetAdress(RouteData routeData) =>
            routeData?.Values["controller"]?.ToString() + ":" + routeData?.Values["action"]?.ToString();
    }
}
