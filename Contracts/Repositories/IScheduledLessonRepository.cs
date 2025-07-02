using SadSchool.Contracts.Data;
using SadSchool.Models.SqlServer;

namespace SadSchool.Contracts.Repositories
{
    public  interface IScheduledLessonRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of all scheduled lessons.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="ScheduledLesson"/> objects representing all scheduled lessons. If no lessons are scheduled,  the list
        /// will be empty.</returns>
        Task<List<ScheduledLesson>> GetAllScheduledLessonsAsync();

        /// <summary>
        /// Retrieves a scheduled lesson by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve details of a specific scheduled lesson. If no lesson is
        /// found with the provided identifier, the method returns <see langword="null"/>.</remarks>
        /// <param name="scheduledLessonId">The unique identifier of the scheduled lesson to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see
        /// cref="ScheduledLesson"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<ScheduledLesson?> GetScheduledLessonByIdAsync(int scheduledLessonId);

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

        /// <summary>
        /// Asynchronously adds a new scheduled lesson to the system.
        /// </summary>
        /// <param name="scheduledLesson">The <see cref="ScheduledLesson"/> object representing the lesson to be added.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the  <see
        /// cref="ScheduledLesson"/> object that was added, including any updates made  by the system (e.g., assigned
        /// identifiers or timestamps).</returns>
        Task<ScheduledLesson?> AddScheduledLessonAsync(ScheduledLesson scheduledLesson);

        /// <summary>
        /// Updates the details of a scheduled lesson in the system.
        /// </summary>
        /// <remarks>Use this method to modify the details of an existing scheduled lesson. Ensure that
        /// the provided  <paramref name="scheduledLesson"/> object contains valid and complete data before calling this
        /// method.</remarks>
        /// <param name="scheduledLesson">The <see cref="ScheduledLesson"/> object containing the updated details of the lesson. Cannot be null.</param>
        /// <returns><see langword="true"/> if the update was successful; otherwise, <see langword="false"/>.</returns>
        Task<bool> UpdateScheduledLessonAsync(ScheduledLesson scheduledLesson);

        /// <summary>
        /// Deletes a scheduled lesson with the specified identifier.
        /// </summary>
        /// <remarks>Use this method to remove a scheduled lesson from the system. Ensure that the
        /// specified <paramref name="scheduledLessonId"/> corresponds to an existing lesson. If the lesson does not
        /// exist, the method will return <see langword="false"/> without throwing an exception.</remarks>
        /// <param name="scheduledLessonId">The unique identifier of the scheduled lesson to delete. Must be a positive integer.</param>
        /// <returns><see langword="true"/> if the scheduled lesson was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteScheduledLessonAsync(int scheduledLessonId);
    }
}
