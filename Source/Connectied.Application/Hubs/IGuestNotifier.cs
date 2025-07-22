namespace Connectied.Application.Hubs;

public interface IGuestNotifier
{
    Task NotifyGuestCreated(string id);
    Task NotifyGuestUpdated(string id);
    Task NotifyGuestDeleted(string id);
}
