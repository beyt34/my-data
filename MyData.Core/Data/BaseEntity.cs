using System;

namespace MyData.Core.Data {
    public class BaseEntity {
        /// <summary>Gets or sets the id.</summary>
        public Guid Id { get; set; }

        /// <summary>Gets or sets a value indicating whether is deleted.</summary>
        public bool IsDeleted { get; set; }

        /// <summary>Gets or sets the created date time.</summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>Gets or sets the created user id.</summary>
        public Guid CreatedUserId { get; set; }

        /// <summary>Gets or sets the updated date time.</summary>
        public DateTime? UpdatedDateTime { get; set; }

        /// <summary>Gets or sets the updated user id.</summary>
        public Guid? UpdatedUserId { get; set; }
    }
}
