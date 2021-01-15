using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyData.Core.Data {
    public interface IRepository<T>
        where T : BaseEntity {
        Task<Guid> AddUpdate(T entity, Guid logUserId);

        Task<T> Find(Expression<Func<T, bool>> predicate);

        Task<T> Detail(Guid id);

        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        FilterModel<T> Filter(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, Expression<Func<T, object>> shortField, bool sortDescending);

        Task<bool> Delete(Guid id, Guid logUserId);

        Task<bool> HardDelete(Guid id);
    }
}
