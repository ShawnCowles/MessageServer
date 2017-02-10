namespace MessageServer.Contracts.Messages
{
    /// <summary>
    /// A message indicating that a client has disconnected.
    /// </summary>
    public class ClientDisconnectedMessage : AbstractMessage
    {
        /// <summary>
        /// The id of the client that has disconnected.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Construct a new ClientDisconnectedMessage.
        /// </summary>
        /// <param name="clientId">The id of the client that has disconnected.</param>
        public ClientDisconnectedMessage(string clientId)
        {
            ClientId = clientId;
        }
    }
}
