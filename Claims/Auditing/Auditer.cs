using System.Threading.Channels;

namespace Claims.Auditing
{
    public class Auditer :IAuditer
    {
        private readonly ChannelWriter<AuditMessage> _writer;
        private readonly TimeProvider _timeProvider;

        public Auditer(Channel<AuditMessage> channel, TimeProvider timeProvider)
        {
            _writer = channel.Writer;
            _timeProvider = timeProvider;
        }

        public void AuditClaim(string id, string httpRequestType)
        {
            Enqueue(AuditEntityType.Claim, id, httpRequestType);
        }

        public void AuditCover(string id, string httpRequestType)
        {
            Enqueue(AuditEntityType.Cover, id, httpRequestType);
        }

        private void Enqueue(AuditEntityType entityType, string id, string httpRequestType)
        {

            var message = new AuditMessage(entityType, id, httpRequestType, _timeProvider.GetUtcNow().UtcDateTime);
            _writer.TryWrite(message);
        }
    }
}
