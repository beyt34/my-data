using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyData.Core.Data {
    public interface IRepository<T>
        where T : BaseEntity {
        Task<Guid> AddUpdateAsync(T entity, Guid logUserId);

        Task<T> DetailAsync(Guid id);

        IQueryable<T> FilterAsync(Expression<Func<T, bool>> predicate);

        FilterModel<T> FilterAsync(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, Expression<Func<T, object>> shortField, bool sortDescending);

        Task<bool> DeleteAsync(Guid id, Guid logUserId);

        Task<bool> HardDeleteAsync(Guid id);

        Task BulkInsertAsync(List<T> entities);
    }
}
