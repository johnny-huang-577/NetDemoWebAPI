using DemoWebAPI.DataAccess.Data;
using DemoWebAPI.DataAccess.Repository.IRepository;
using DemoWebAPI.Models.Entity;

namespace DemoWebAPI.DataAccess.Repository
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly DemoWebAPIDbContext _db;

        public TeacherRepository(DemoWebAPIDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
