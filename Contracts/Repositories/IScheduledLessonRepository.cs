using SadSchool.Contracts.Data;
using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public  interface IScheduledLessonRepository : IBaseRepository
    {
        /// <summary>
        /// Retrieves the scheduled lesson associated with the specified start time identifier.
        /// </summary>
        /// <param name="startTimeId">The unique identifier of the start time for which the scheduled lesson is to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see
        /// cref="ScheduledLesson"/> associated with the specified start time identifier, or <see langword="null"/> if
        /// no lesson is found.</returns>
        Task<List<ScheduledLesson>> GetScheduledLessonsByStartTimeIdAsync(int startTimeId);

        /// <summary>
        /// Asynchronously retrieves the scheduled lesson associated with the specified subject ID.
        /// </summary>
        /// <param name="subjectId">The unique identifier of the subject for which the scheduled lesson is to be retrieved.  Must be a positive
        /// integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see
        /// cref="ScheduledLesson"/> associated with the specified subject ID, or <see langword="null"/>  if no
        /// scheduled lesson is found.</returns>
        Task<List<ScheduledLesson>> GetScheduledLessonsBySubjectIdAsync(int subjectId);

        /// <summary>
        /// Asynchronously retrieves the scheduled lesson associated with the specified class ID.
        /// </summary>
        /// <remarks>Use this method to retrieve the scheduled lesson details for a specific class. Ensure
        /// that the <paramref name="classId"/> provided is valid and corresponds to an existing class.</remarks>
        /// <param name="classId">The unique identifier of the class for which the scheduled lesson is to be retrieved. Must be a positive
        /// integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of  <see
        /// cref="ScheduledLesson"/> associated with the specified class ID,  or <see langword="null"/> if no scheduled
        /// lesson is found.</returns>
        Task<List<ScheduledLesson>> GetScheduledLessonsByClassIdAsync(int classId);

        /// <summary>
        /// Asynchronously retrieves the scheduled lesson for a specific teacher by their unique identifier.
        /// </summary>
        /// <remarks>Use this method to fetch the scheduled lesson for a teacher based on their unique
        /// identifier. Ensure that the <paramref name="teacherId"/> is valid and corresponds to an existing
        /// teacher.</remarks>
        /// <param name="teacherId">The unique identifier of the teacher whose scheduled lesson is to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see
        /// cref="ScheduledLesson"/> associated with the specified teacher, or <see langword="null"/> if no scheduled
        /// lesson is found.</returns>
        Task<List<ScheduledLesson>> GetScheduledLessonsByTeacherIdAsync(int teacherId);

        /// <summary>
        /// Retrieves a list of scheduled lessons for a specified date.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the scheduled lessons for the
        /// given date.  Ensure that the provided date string is in the correct format to avoid exceptions.</remarks>
        /// <param name="date">The date for which to retrieve scheduled lessons, in the format "yyyy-MM-dd".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="ScheduledLesson"/> objects scheduled for the specified date. If no lessons are scheduled,  the list
        /// will be empty.</returns>
        Task<List<ScheduledLesson>> GetScheduledLessonsByDayAsync(Days day);
    }
}
