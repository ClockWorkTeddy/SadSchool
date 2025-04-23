// <copyright file="LoggerJobService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.HangFire
{
    using Serilog;

    /// <summary>
    /// Service for logging job details.
    /// </summary>
    public class LoggerJobService
    {
        /// <summary>
        /// Logs the job details.
        /// </summary>
        public void LogJob()
        {
            // Log the job details
            Log.Information($"Job Time: {DateTime.Now}");
        }
    }
}
