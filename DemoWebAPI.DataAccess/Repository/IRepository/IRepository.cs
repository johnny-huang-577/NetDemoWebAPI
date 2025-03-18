using System.Linq.Expressions;

namespace DemoWebAPI.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //獲取單一筆資料，並且支援篩選、包括關聯資料等選項
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null,
                         string? includeProperties = null,
                         bool tracked = true);

        //獲取所有的資料，並且支援篩選、排序、包括關聯資料等選項
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
                                         Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
                                         string? includeProperties = null,
                                         bool tracked = true);

        //新增一筆資料
        Task AddAsync(T entity);

        //新增多筆資料
        Task AddRangeAsync(IEnumerable<T> entity);

        //刪除一筆資料
        Task RemoveAsync(T entity);

        //刪除多筆資料
        Task RemoveRangeAsync(IEnumerable<T> entity);

        //(?)
        Task AttachAsync(T entity);

        //更新一筆資料
        Task UpdateAsync(T entity);

        //批量更新多筆資料
        Task UpdateRangeAsync(IEnumerable<T> entity);

        //檢查資料庫中是否有符合條件的資料
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);


    }
}
