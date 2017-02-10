using TypeLite;

namespace MessageServer.Contracts.Messages
{
    /// <summary>
    /// Base class for any message that can be sent along the MessageBus.
    /// </summary>
    [TsClass]
    public abstract class AbstractMessage
    {
    }
}
