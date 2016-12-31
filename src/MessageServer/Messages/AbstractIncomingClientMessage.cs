using System.Runtime.Serialization;
using TypeLite;

namespace MessageServer.Messages
{
    [TsClass]
    [DataContract]
    public abstract class AbstractIncomingClientMessage : AbstractClientMessage
    {
        // Filled in by WebSocketConnectionService after deserialization
        [TsIgnore]
        public string ClientId { get; set; }

        protected AbstractIncomingClientMessage(string messageName)
            : base(messageName)
        {
        }
    }
}
