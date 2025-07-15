namespace SadSchool.Repositories
{
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
    using SadSchool.DbContexts;

    /// <summary>
    /// Repository for managing start times (schedule positions) in the SadSchool database.
    /// </summary>
    /// <param name="context">DbContext instance.</param>
    /// <param name="cacheService">Cache service instance.</param>
    public class StartTimeRepository(SadSchoolContext context, ICacheService cacheService)
        : BaseRepository(context, cacheService), IStartTimeRepository
    {
    }
}
