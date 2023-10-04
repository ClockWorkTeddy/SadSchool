namespace SadSchool.Services
{
    public interface INavigationService
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public void RefreshBackParams(RouteData routeData);
    }

    public struct UrlParams
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }

    public class NavigationService : INavigationService
    {
        public string Controller { get; set; }
        public string Action { get; set; }

        private Dictionary<string, UrlParams> Map = new Dictionary<string, UrlParams>();

        public NavigationService()
        {
            Map["Home:About"] = new UrlParams { Controller = "Home", Action = "Index" };
            Map["Data:DataIndex"] = new UrlParams { Controller = "Home", Action = "Index" };
                Map["Class:Classes"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Class:Add"] = new UrlParams { Controller = "Class", Action = "Classes" };
                    Map["Class:Edit"] = new UrlParams { Controller = "Class", Action = "Classes" };
                Map["Lesson:Lessons"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Lesson:Add"] = new UrlParams { Controller = "Lesson", Action = "Lessons" };
                    Map["Lesson:Edit"] = new UrlParams { Controller = "Lesson", Action = "Lessons" };
                Map["ScheduledLesson:ScheduledLessons"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["ScheduledLesson:Add"] = new UrlParams { Controller = "ScheduledLesson", Action = "ScheduledLessons" };
                    Map["ScheduledLesson:Edit"] = new UrlParams { Controller = "ScheduledLesson", Action = "ScheduledLessons" };
                Map["Mark:Marks"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Mark:Add"] = new UrlParams { Controller = "Mark", Action = "Marks" };
                    Map["Mark:Edit"] = new UrlParams { Controller = "Mark", Action = "Marks" };
                Map["StartTime:StartTimes"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["StartTime:Add"] = new UrlParams { Controller = "StartTime", Action = "StartTimes" };
                    Map["StartTime:Edit"] = new UrlParams { Controller = "StartTime", Action = "StartTimes" };
                Map["Student:Students"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Student:Add"] = new UrlParams { Controller = "Student", Action = "Students" };
                    Map["Student:Edit"] = new UrlParams { Controller = "Student", Action = "Students" };
                Map["Subject:Subjects"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Subject:Add"] = new UrlParams { Controller = "Subject", Action = "Subjects" };
                    Map["Subject:Edit"] = new UrlParams { Controller = "Subject", Action = "Subjects" };
                Map["Teacher:Teachers"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
                    Map["Teacher:Add"] = new UrlParams { Controller = "Teacher", Action = "Teachers" };
                    Map["Teacher:Edit"] = new UrlParams { Controller = "Teacher", Action = "Teachers" };
                Map["Schedule:GetSchedule"] = new UrlParams { Controller = "Data", Action = "DataIndex" };
            Map["Stuff:Stuff"] = new UrlParams { Controller = "Home", Action = "Index" };
                Map["Users:Users"] = new UrlParams { Controller = "Stuff", Action = "Stuff" };
                    Map["Users:Register"] = new UrlParams { Controller = "Users", Action = "Users" };
                    Map["Users:RolesProcessing"] = new UrlParams { Controller = "Users", Action = "Users" };
                        Map["Users:AddRole"] = new UrlParams { Controller = "Users", Action = "RolesProcessing" };
        }

        public void RefreshBackParams(RouteData routeData)
        {
            string adress = GetAdress(routeData);

            Controller = Map[adress].Controller;
            Action = Map[adress].Action;
        }

        private string GetAdress(RouteData routeData)
        {
            return routeData?.Values["controller"]?.ToString() + ":" + routeData?.Values["action"]?.ToString();
        }
    }
}
