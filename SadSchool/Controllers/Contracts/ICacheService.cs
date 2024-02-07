namespace SadSchool.Controllers.Contracts
{
    public interface ICacheService
    {
        List<T> GetObject<T>(int id) where T : class;
        void RefreshObject<T>(T obj) where T : class;
    }
}
