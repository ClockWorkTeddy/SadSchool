using Serilog;

namespace SadSchool.Services.HangFire
{
    public class LoggerJobService
    {
        public void LogJob()
        {
            // Log the job details
            Log.Information($"Job Time: {DateTime.Now}");
        }
    }
}
