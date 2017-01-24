namespace MessageServer.Contracts.Messages
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
