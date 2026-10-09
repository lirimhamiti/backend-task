using System.Threading.Channels;
using Claims.Auditing;
using Xunit;

namespace Claims.Tests;

public class AuditerTests
{

    [Fact]
    public void AuditClaim_PutsMessageOnQueue()
    {
        var queue = Channel.CreateUnbounded<AuditMessage>();
        var auditer = new Auditer(queue, TimeProvider.System);

        auditer.AuditClaim("claim-1", "POST");

        Assert.True(queue.Reader.TryRead(out var message));
        Assert.Equal("claim-1", message.EntityId);
        Assert.Equal("POST", message.HttpRequestType);
        Assert.Equal(AuditEntityType.Claim, message.EntityType);
    }
}
