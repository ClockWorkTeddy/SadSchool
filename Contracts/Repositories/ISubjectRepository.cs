namespace SadSchool.Contracts.Repositories
{
    using SadSchool.Models.SqlServer;

    public interface ISubjectRepository : IBaseRepository
    {
        Task<Subject?> GetSubjectByNameAsync(string name);
    }
}
