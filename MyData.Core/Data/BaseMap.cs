using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyData.Core.Data {
    public class BaseMap<T>
    where T : BaseEntity {
        public BaseMap(EntityTypeBuilder<T> builder) {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.IsDeleted).IsRequired();
            builder.Property(w => w.CreatedDateTime).IsRequired();
            builder.Property(w => w.CreatedUserId).IsRequired();
            builder.Property(w => w.UpdatedDateTime);
            builder.Property(w => w.UpdatedUserId);
        }
    }
}
