using SadSchool.DbContexts;
using SadSchool.Models.SqlServer;

namespace SadSchool.Controllers.GraphQl
{
    public class Mutation
    {
        public Class CreateClass(SadSchoolContext context, string name)
        {
            var newClass = new Class()
            {
                Name = name
            };
            context.Classes.Add(newClass);
            context.SaveChanges();
            return newClass;
        }

        public Lesson CreateLesson(SadSchoolContext context, Lesson newLesson)
        {
            context.Lessons.Add(newLesson);
            context.SaveChanges();
            return newLesson;
        }

        public Student CreateStudent(SadSchoolContext context, Student newStudent)
        {
            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudent;
        }
    }
}
