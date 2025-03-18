

namespace DemoWebAPI.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
        ITeacherRepository Teacher { get; }

    }
}
