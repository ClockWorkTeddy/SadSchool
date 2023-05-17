namespace SadSchool.Services
{
    public interface ILoginDisplay
    {
        public bool DisplayForm { get; set; }
    }
    public class LoginDisplayService : ILoginDisplay
    {
        public bool DisplayForm { get; set; } = false;
    }
}
