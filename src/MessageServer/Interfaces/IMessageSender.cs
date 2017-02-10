using MessageServer.Contracts.Messages;

namespace MessageServer.Interfaces
{
    /// <summary>
    /// An interface for a service that can send messages on the MessageBus. Registered by 
    /// default in MessageServerModule.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Send a message onto the MessageBus
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(AbstractMessage message);
    }
}
