using SadSchool.Models.SqlServer;
using SadSchool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadSchool.Contracts
{
    public interface ITeacherMapper
    {
        public TeacherViewModel ToViewModel(Teacher teacher);
        public Teacher ToModel(TeacherViewModel teacherViewModel);
    }
}
