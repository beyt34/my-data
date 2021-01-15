using MyData.Core.Data;

namespace MyData.Data.Domain {
    public class City : BaseEntity {
        /// <summary>Gets or sets the code.</summary>
        public string Code { get; set; }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }
    }
}
