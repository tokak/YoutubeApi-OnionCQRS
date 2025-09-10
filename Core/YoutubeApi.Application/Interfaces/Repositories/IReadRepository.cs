using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using YoutubeApi.Domain.Common;

namespace YoutubeApi.Application.Interfaces.Repositories
{
    public interface IReadRepository<T> where T : class, IEntityBase, new()
    {
        // GetAllAsync metodu: Veritabanından kayıtları liste halinde asenkron çeker.
        // Parametreler:
        // - predicate: Filtre şartı (örnek: x => x.Aktif == true)
        // - include: İlişkili tabloları dahil etmek için (Include işlemleri ve theninclude)
        // - orderBy: Çekilen kayıtları sıralamak için
        // - enableTracking: EF Core'un tracking (değişiklik takibi) özelliğini aç/kapat
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false);

        Task<IList<T>> GetAllByPagingAsync(Expression<Func<T, bool>>? predicate = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           bool enableTracking = false, int currentPage = 1, int pageSize = 3);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
            bool enableTracking = false);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    }
}
