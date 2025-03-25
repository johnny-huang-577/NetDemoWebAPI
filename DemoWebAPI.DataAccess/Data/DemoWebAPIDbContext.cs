using DemoWebAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAPI.DataAccess.Data
{
    public class DemoWebAPIDbContext : DbContext
    {
        public DemoWebAPIDbContext(DbContextOptions<DemoWebAPIDbContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }


        //public override int SaveChanges()
        //{
        //    // 自動設定建立日期與修改日期
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        var entity = (BaseEntity)entityEntry.Entity;

        //        // 修改時設定修改時間
        //        entity.UpdateTime = DateTime.Now;

        //        // 新增時設定建立時間
        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            entity.CreateTime = DateTime.Now;
        //        }
        //    }

        //    return base.SaveChanges();
        //}
    }
}
