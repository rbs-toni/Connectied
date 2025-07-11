using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectied.Domain;
public abstract class Entity
{
    readonly List<BaseEvent> _domainEvents = new();

    protected Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

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
