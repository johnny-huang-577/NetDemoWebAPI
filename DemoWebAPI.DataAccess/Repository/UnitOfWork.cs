using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository;
using DemoWebAPI.DataAccess.Repository.IRepository;
using DemoWebAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;


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
            var entries = _db.ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreateTime = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdateTime = DateTime.Now;
                }
            }

            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

}
