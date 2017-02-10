using System.Runtime.Serialization;
using TypeLite;

namespace MessageServer.Contracts.Messages
{
    /// <summary>
    /// Abstract base class for a message that can enter the server through the
    /// WebsocketConnectionService. All subclasses should be listed out through an
    /// IIncomingMessageTypeProvider provided to WebsocketConnectionService. 
    /// </summary>
    [TsClass]
    [DataContract]
    public abstract class AbstractIncomingClientMessage : AbstractMessage
    {
        /// <summary>
        /// The id of the originating client. Will be filled in by WebsocketConnectionService
        /// after deserialization. Useful for addressing return messages.
        /// </summary>
        [TsIgnore]
        public string ClientId { get; set; }
    }
}
