namespace MessageServer.Contracts.Messages
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
