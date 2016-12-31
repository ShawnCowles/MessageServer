using MessageServer.Messages;

namespace MessageServer.Messages
{
    public class ClientDisconnectedMessage : AbstractMessage
    {
        public string ClientId { get; }

        public ClientDisconnectedMessage(string clientId)
        {
            ClientId = clientId;
        }
    }
}
