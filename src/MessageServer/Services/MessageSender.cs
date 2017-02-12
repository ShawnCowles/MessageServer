using System;
using MessageServer.Contracts.Messages;
using MessageServer.Interfaces;

namespace MessageServer.Services
{
    /// <summary>
    /// Default implementation of IMessageSender, has the ability to send messages on the
    /// MessageBus without having to be a bus service.
    /// </summary>
    public class MessageSender : IMessageSender
    {
        private MessageBus _messageBus;

        /// <summary>
        /// Register the message bus to send messages on. Handled within MessageBus itself.
        /// </summary>
        /// <param name="messageBus">The message bus to send messages on.</param>
        public void RegisterBus(MessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        /// <summary>
        /// Send a message onto the MessageBus
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(AbstractMessage message)
        {
            if(_messageBus == null)
            {
                throw new InvalidOperationException("Cannot send a message before the MessageBus has been registered.");
            }

            _messageBus.SendMessage(message);
        }
    }
}
