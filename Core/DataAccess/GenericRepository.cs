using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        private TContext Context { get; }


        public GenericRepository(TContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> Table => Context.Set<TEntity>();

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, bool tracking = true)
        {
            var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {


            await Table.AddAsync(entity);
            return entity;
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {


            await Table.AddRangeAsync(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {

            Table.Update(entity);
            return entity;
        }

        public List<TEntity> UpdateRange(List<TEntity> entities)
        {


            Context.UpdateRange(entities);
            Context.SaveChanges();
            return entities;
        }

        public async Task<TEntity?> RemoveAsync(int id)
        {
            var entity = await Table.FirstOrDefaultAsync(p => p.Id == id);
            if (entity != null)
            {
                Remove(entity);
                return entity;
            }

            return null;
        }

        public TEntity Remove(TEntity entity)
        {

            Table.Remove(entity);
            return entity;
        }

        public List<TEntity> RemoveRange(List<TEntity> entities)
        {

            Table.RemoveRange(entities);
            Context.SaveChanges();
            return entities;
        }

        public async Task<int> SaveAsync()
            => await Context.SaveChangesAsync();


        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracking = true)
        {
            var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool tracking = true)
         => await GetAsync(entity => entity.Id == id, tracking);

        public IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>>? filter = null, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<TEntity?> SoftRemoveAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);

            if (entityToDelete != null)
            {
                entityToDelete.IsDeleted = true; 
            }

            return entityToDelete;
        }

        public PaginationResult<TEntity> GetWithPagination(BasePaginationRequest req, Expression<Func<TEntity, bool>>? filter = null)
        {
            var query = filter != null ? Table.Where(filter) : Table;
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / req.PageSize);
            var entities = query.Skip((req.Page - 1) * req.PageSize).Take(req.PageSize).ToList();
            return new PaginationResult<TEntity>(entities, totalCount, totalPages, req.Page);
        }
    }
}
