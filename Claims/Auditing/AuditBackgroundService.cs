using System.Threading.Channels;

namespace Claims.Auditing
{
    public class AuditBackgroundService : BackgroundService
    {

        private readonly ChannelReader<AuditMessage> _reader;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AuditBackgroundService> _logger;

        public AuditBackgroundService(
            Channel<AuditMessage> channel,
            IServiceScopeFactory scopeFactory,
            ILogger<AuditBackgroundService> logger)
        {
            _reader = channel.Reader;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in _reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

                    context.Add(ToEntity(message));
                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("Audit saved: {EntityType} {EntityId} {HttpRequestType}",
                        message.EntityType, message.EntityId, message.HttpRequestType);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Failed to save audit log for {EntityType} {EntityId}",
                        message.EntityType, message.EntityId);
                }
            }
        }

        private static object ToEntity(AuditMessage message) => message.EntityType switch
        {
            AuditEntityType.Claim => new ClaimAudit
            {
                ClaimId = message.EntityId,
                HttpRequestType = message.HttpRequestType,
                Created = message.Created
            },
            AuditEntityType.Cover => new CoverAudit
            {
                CoverId = message.EntityId,
                HttpRequestType = message.HttpRequestType,
                Created = message.Created
            },
            _ => throw new ArgumentOutOfRangeException(nameof(message), message.EntityType, "Unknown audit entity type")
        };
    }
}
