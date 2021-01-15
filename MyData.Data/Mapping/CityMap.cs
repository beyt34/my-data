using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyData.Core.Data;
using MyData.Data.Domain;

namespace MyData.Data.Mapping {
    public class CityMap : BaseMap<City> {
        public CityMap(EntityTypeBuilder<City> builder)
            : base(builder) {
            builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        }
    }
}
