using Application.Notifications;
using Contracts;
using MediatR;

namespace Application.Handlers;

internal sealed class EmailHandler : INotificationHandler<CompanyDeletedNotification>
{
    private readonly ILoggerManager _logger;

    public EmailHandler(ILoggerManager logger) => _logger = logger;

    public Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogWarn($"Delete action for the company with: {notification.Id} has occurred.");
        
        return Task.CompletedTask;
    }
}
