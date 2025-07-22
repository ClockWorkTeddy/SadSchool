namespace SadSchool.Contracts.Repositories
{
    public interface IRepositories
    {
        /// <summary>
        /// Gets the collection of base repositories associated with the current context.
        /// </summary>
        IIndependentRepositories IndependentRepositories { get; }

        /// <summary>
        /// Gets the collection of derived repositories associated with the current context.
        /// </summary>
        IDerivedRepositories DerivedRepositories { get; }
    }
}
