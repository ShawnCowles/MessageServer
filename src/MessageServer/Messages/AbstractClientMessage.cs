using System.Runtime.Serialization;
using TypeLite;

namespace MessageServer.Messages
{
    [TsClass]
    [DataContract]
    public abstract class AbstractClientMessage : AbstractMessage
    {
        [DataMember]
        public string MessageName { get; private set; }

        protected AbstractClientMessage(string messageName)
        {
            MessageName = messageName;
        }
    }
}
