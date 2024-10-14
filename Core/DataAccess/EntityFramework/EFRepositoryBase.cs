using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public async Task AddAsync(TEntity entity)
        {
            await using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            await context.SaveChangesAsync();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate, bool tracking)
        {
            using var context = new TContext();
            if (!tracking)
                return context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public List<TEntity> GetAll(bool tracking, Expression<Func<TEntity, bool>>? expression = null)
        {
            using var context = new TContext();
            if (!tracking)
                return expression == null ? context.Set<TEntity>().AsNoTracking().ToList() : context.Set<TEntity>().AsNoTracking().Where(expression).ToList();
            return expression == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(expression).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync(bool tracking, Expression<Func<TEntity, bool>>? expression = null)
        {
            using var context = new TContext();
            if (!tracking)
                return expression == null ? await context.Set<TEntity>().AsNoTracking().ToListAsync() : await context.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
            return expression == null ? await context.Set<TEntity>().ToListAsync() : await context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool tracking)
        {
            using var context = new TContext();
            if (!tracking)
                return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public TEntity GetById<T>(T id)
        {
            using var context = new TContext();
            return context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync<T>(T id)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            using var context = new TContext();
            var removeEntity = context.Entry(entity);
            removeEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
