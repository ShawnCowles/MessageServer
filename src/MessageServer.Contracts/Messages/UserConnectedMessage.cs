namespace MessageServer.Contracts.Messages
{
    /// <summary>
    /// A message indicating that a new client has connected.
    /// </summary>
    public class ClientConnectedMessage : AbstractMessage
    {
        /// <summary>
        /// The id of the client that connected.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Construct a new ClientConnectedMessage.
        /// </summary>
        /// <param name="clientId">The id of the client that connected.</param>
        public ClientConnectedMessage(string clientId)
        {
            ClientId = clientId;
        }
    }
}
