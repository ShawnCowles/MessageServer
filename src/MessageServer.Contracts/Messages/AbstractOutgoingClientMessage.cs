using System.Runtime.Serialization;
using TypeLite;

namespace MessageServer.Contracts.Messages
{
    /// <summary>
    /// Base class for a message that can be sent through the WebsocketConnectionService to
    /// a client.
    /// </summary>
    [TsClass]
    [DataContract]
    public abstract class AbstractOutgoingClientMessage : AbstractMessage
    {
        /// <summary>
        /// The Id(s) of the clients to send the message to.
        /// </summary>
        [TsIgnore]
        public string[] ClientIds { get; }

        /// <summary>
        /// Construct an AbstractOutgoingClientMessage addressed to multiple clients.
        /// </summary>
        /// <param name="clientIds">The ids of the clients to send the message to.</param>
        protected AbstractOutgoingClientMessage(string[] clientIds)
        {
            ClientIds = clientIds;
        }

        /// <summary>
        /// Construct an AbstractOutgoingClientMessage addressed to a single client.
        /// </summary>
        /// <param name="clientId">The id of the client to send the message to.</param>
        protected AbstractOutgoingClientMessage(string clientId)
        {
            ClientIds = new[] { clientId };
        }
    }
}
