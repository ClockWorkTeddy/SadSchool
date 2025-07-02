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
        /// Retrieves a list of objects of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of objects to retrieve. Must be a reference type.</typeparam>
        /// <returns>A list of objects of type <typeparamref name="T"/>, or <see langword="null"/> if no objects are available.</returns>
        List<T>? GetObjects<T>()
            where T : class;

        /// <summary>
        /// Sets the collection of objects to be processed or managed.
        /// </summary>
        /// <remarks>The method replaces any existing collection with the provided list of objects. 
        /// Ensure that the list is not null before calling this method.</remarks>
        /// <typeparam name="T">The type of objects in the collection. Must be a reference type.</typeparam>
        /// <param name="objects">The list of objects to set. Cannot be null.</param>
        void SetObjects<T>(List<T> objects)
            where T : class;

        /// <summary>
        /// Method returns an object from cache using specified id.
        /// </summary>
        /// <typeparam name="T">Type of returned object.</typeparam>
        /// <param name="id">Id of returned object.</param>
        /// <returns>Return List of objects of type T with specified id. In case of one object List.Count === 1.</returns>
        T? GetObject<T>(int id)
            where T : class;

        /// <summary>
        /// The method refreshed object data if object data was refreshed by user.
        /// </summary>
        /// <typeparam name="T">Type of refreshing object.</typeparam>
        /// <param name="obj">Refreshing object.</param>
        void SetObject<T>(T obj)
            where T : class;

        void RemoveObject<T>(T obj)
            where T : class;

        void RemoveObjects<T>()
            where T : class;
    }
}
