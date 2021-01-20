using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace MyData.Core.Data {
    public class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext {
        private readonly TContext context;

        public Repository(TContext context) {
            this.context = context;
        }

        public async Task<Guid> AddUpdateAsync(TEntity entity, Guid logUserId) {
            var obj = await DetailAsync(entity.Id);
            if (obj == null) {
                entity.IsDeleted = false;
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.CreatedUserId = logUserId;
                await context.Set<TEntity>().AddAsync(entity);
            } else {
                entity.UpdatedDateTime = DateTime.UtcNow;
                entity.UpdatedUserId = logUserId;
                context.Entry(entity).State = EntityState.Modified;
            }

            try {
                await context.SaveChangesAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return Guid.Empty;
            }

            return entity.Id;
        }

        public async Task<TEntity> DetailAsync(Guid id) {
            return await context.Set<TEntity>().Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> FilterAsync(Expression<Func<TEntity, bool>> predicate) {
            predicate = predicate.And(q => !q.IsDeleted);
            return context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public FilterModel<TEntity> FilterAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, object>> shortField, bool sortDescending) {
            var retval = new FilterModel<TEntity>();
            predicate = predicate.And(q => !q.IsDeleted);

            if (shortField == null) {
                shortField = arg => (object)arg.CreatedDateTime;
                sortDescending = true;
            }

            var filter = context.Set<TEntity>().Where(predicate).AsQueryable();
            retval.Total = filter.Count();
            filter = sortDescending ? filter.OrderByDescending(shortField) : filter.OrderBy(shortField);
            filter = filter.Skip(pageIndex * pageSize).Take(pageSize);
            retval.Data = filter.AsQueryable();

            return retval;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid logUserId) {
            var obj = await DetailAsync(id);
            if (obj == null) {
                return false;
            }

            obj.IsDeleted = true;
            obj.UpdatedDateTime = DateTime.UtcNow;
            obj.UpdatedUserId = logUserId;

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteAsync(Guid id) {
            var obj = await context.Set<TEntity>().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (obj == null) {
                return false;
            }

            context.Set<TEntity>().Remove(obj);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task BulkInsertAsync(List<TEntity> entities) {
            await context.BulkInsertAsync(entities);
        }
    }
}
