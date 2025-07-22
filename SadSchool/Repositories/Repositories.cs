namespace SadSchool.Repositories
{
    using SadSchool.Contracts.Repositories;

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
