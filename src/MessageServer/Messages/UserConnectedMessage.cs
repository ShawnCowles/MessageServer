using MessageServer.Messages;

namespace MessageServer.Messages
{
    public class ClientConnectedMessage : AbstractMessage
    {
        public string ClientId { get; }

        public ClientConnectedMessage(string clientId)
        {
            ClientId = clientId;
        }
    }
}
