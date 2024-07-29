// <copyright file="ICacheService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Contracts
{
    /// <summary>
    /// Interface for cache services those are used in controlles.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Method returns an object from cache using specified id.
        /// </summary>
        /// <typeparam name="T">Type of returned object.</typeparam>
        /// <param name="id">Id of returned object.</param>
        /// <returns>Return List of objects of type T with specified id. In case of one object List.Count === 1.</returns>
        List<T?> GetObject<T>(int id)
            where T : class;

        /// <summary>
        /// The method refreshed object data if object data was refreshed by user.
        /// </summary>
        /// <typeparam name="T">Type of refreshing object.</typeparam>
        /// <param name="obj">Refreshing object.</param>
        void RefreshObject<T>(T obj)
            where T : class;
    }
}
