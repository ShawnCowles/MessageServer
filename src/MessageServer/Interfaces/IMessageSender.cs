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
        /// Register the message bus to send messages on. Handled within MessageBus itself.
        /// </summary>
        /// <param name="messageBus">The message bus to send messages on.</param>
        void RegisterBus(MessageBus messageBus);


        /// <summary>
        /// Send a message onto the MessageBus
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(AbstractMessage message);
    }
}
