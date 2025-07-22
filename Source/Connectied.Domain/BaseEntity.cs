using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Connectied.Domain;
public abstract class BaseEntity
{
    readonly List<BaseEvent> _domainEvents = new();

    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    [MaxLength(36)]
    public string Id { get; set; }

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
}
