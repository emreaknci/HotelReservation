﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        DbSet<TEntity> Table { get; }
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);
        TEntity Update(TEntity entity);
        List<TEntity> UpdateRange(List<TEntity> entities);
        Task<TEntity?> RemoveAsync(int id);
        TEntity Remove(TEntity entity);
        List<TEntity> RemoveRange(List<TEntity> entities);
        Task<int> SaveAsync();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, bool tracking = true);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracking = true);
        Task<TEntity?> GetByIdAsync(int id, bool tracking = true);

        IQueryable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>>? filter = null, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity?> SoftRemoveAsync(int id);
        PaginationResult<TEntity> GetWithPagination(BasePaginationRequest req, Expression<Func<TEntity, bool>>? filter = null);

    }
}
