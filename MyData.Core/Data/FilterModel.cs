using System.Linq;

namespace MyData.Core.Data {
    public class FilterModel<T>
    where T : BaseEntity {
        public int Total { get; set; }

        public IQueryable<T> Data { get; set; }
    }
}
