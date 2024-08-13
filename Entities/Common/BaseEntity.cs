using Core.Entities;

namespace Entities.Common
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
