// <copyright file="Repositories.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Repositories
{
    using SadSchool.Contracts.Repositories;

    /// <summary>
    /// Provides access to both independent and derived repositories.
    /// </summary>
    /// <remarks>This class serves as a composite repository, encapsulating both independent and derived
    /// repositories. It implements the <see cref="IRepositories"/> interface, allowing clients to interact with a
    /// unified repository interface.</remarks>
    /// <param name="independentRepositories">The independent repositories object. Independent repos are repos, whom entities don't depend on the other entities.</param>
    /// <param name="derivedRepositories">The derived repositories object. Derived repos are repos, whom entities depend on the other entities.</param>
    public class Repositories(
        IIndependentRepositories independentRepositories,
        IDerivedRepositories derivedRepositories) : IRepositories
    {
        /// <inheritdoc/>
        public IIndependentRepositories IndependentRepositories { get; } = independentRepositories;

        /// <inheritdoc/>
        public IDerivedRepositories DerivedRepositories { get; } = derivedRepositories;
    }
}
