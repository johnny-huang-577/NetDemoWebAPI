using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository;
using DemoWebAPI.DataAccess.Repository.IRepository;


namespace TestBackEnd.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DemoWebAPIDbContext _db;
        public ITeacherRepository Teacher { get; private set; }


        public UnitOfWork(DemoWebAPIDbContext db)
        {
            _db = db;
            Teacher = new TeacherRepository(_db);

        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

}
