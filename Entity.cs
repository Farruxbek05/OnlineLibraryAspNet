using OnlineLibraryAspNet.Interfice;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibraryAspNet
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
