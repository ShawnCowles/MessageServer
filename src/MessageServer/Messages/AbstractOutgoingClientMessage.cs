using System.Runtime.Serialization;
using TypeLite;

namespace MessageServer.Messages
{
    [TsClass]
    [DataContract]
    public abstract class AbstractOutgoingClientMessage : AbstractClientMessage
    {
        [TsIgnore]
        public string[] ClientIds { get; }

        protected AbstractOutgoingClientMessage(string messageName, string[] clientIds)
            : base(messageName)
        {
            ClientIds = clientIds;
        }

        protected AbstractOutgoingClientMessage(string messageName, string clientId)
            : base(messageName)
        {
            ClientIds = new[] { clientId };
        }
    }
}
