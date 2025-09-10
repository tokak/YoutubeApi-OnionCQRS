﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YoutubeApi.Application.Interfaces.Repositories;
using YoutubeApi.Domain.Common;
using YoutubeApi.Persistence.Context;

namespace YoutubeApi.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _appDbContext;

        public ReadRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private DbSet<T> Table { get => _appDbContext.Set<T>(); }

        /// <summary>
        /// Veritabanından kayıtları asenkron olarak çeker. 
        /// Filtre, include, sıralama ve tracking ayarlarını destekler.
        /// </summary>
        /// <param name="predicate">Filtre şartı (örnek: x => x.Aktif == true)</param>
        /// <param name="include">İlişkili tabloları dahil etmek için (Include işlemleri)</param>
        /// <param name="orderBy">Çekilen kayıtları sıralamak için</param>
        /// <param name="enableTracking">EF Core tracking (değişiklik takibi) özelliğini aç/kapat</param>
        /// <returns>Filtre, include ve sıralama uygulanmış liste döner</returns>
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync();

            return await queryable.ToListAsync();
        }
        public async Task<IList<T>> GetAllByPagingAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false, int currentPage = 1, int pageSize = 3)
        {
            IQueryable<T> queryable = Table;
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return await queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);

            //queryable.Where(predicate);

                return await queryable.FirstOrDefaultAsync(predicate);
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            Table.AsNoTracking();
            if (predicate is not null) Table.Where(predicate).Count();
           
            return await Table.CountAsync();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate,bool enableTracking=false)
        {
            if (!enableTracking) Table.AsNoTracking();
            return  Table.Where(predicate);
       
        }



      
    }
}
