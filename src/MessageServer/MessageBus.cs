using System.Collections.Generic;
using System.Linq;
using MessageServer.Interfaces;
using MessageServer.Contracts.Messages;

namespace MessageServer
{
    /// <summary>
    /// The MessageBus at the core of MessageServer. It passes messages between IBusServices.
    /// </summary>
    public class MessageBus
    {
        private static readonly object LOCK = new object();
        
        private readonly IBusService[] _busServices;
        private readonly IMessageSender _messageSender;

        private readonly Dictionary<IBusService, List<AbstractMessage>> _queues;

        /// <summary>
        /// Construct a new MessageBus.
        /// </summary>
        /// <param name="services">The services that will be operating on the bus.</param>
        /// <param name="messageSender">The message sender to link with this MessageBus</param>
        public MessageBus(IEnumerable<IBusService> services, IMessageSender messageSender)
        {
            _busServices = services.ToArray();
            _messageSender = messageSender;
            
            _queues = new Dictionary<IBusService, List<AbstractMessage>>();
            
            foreach (var service in services)
            {
                _queues.Add(service, new List<AbstractMessage>());
            }
        }

        /// <summary>
        /// Start the message bus, and all of the services operating on it.
        /// </summary>
        public void Start()
        {
            _messageSender.RegisterBus(this);

            foreach (var service in _busServices)
            {
                service.Start(this);
            }
        }

        /// <summary>
        /// Stop the message bus, and all of the services operating on it.
        /// </summary>
        public void Stop()
        {
            foreach (var service in _busServices)
            {
                service.Stop();
            }
        }

        /// <summary>
        /// Send a message on the bus, routing it to all services with a matching type in their
        /// MatchingMessageTypes.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(AbstractMessage message)
        {
            lock (LOCK)
            {
                var messageType = message.GetType();

                foreach (var service in _queues.Keys)
                {
                    if (TypeUtils.TypesMatch(service.MatchingMessageTypes, messageType))
                    {
                        _queues[service].Add(message);
                    }
                }
            }
        }
        
        /// <summary>
        /// Get a list of all of the messages queued for a service, and clear the queue.
        /// </summary>
        /// <param name="service">The service to get messages for.</param>
        /// <returns>All of the messages queued for this service since the last check.</returns>
        public AbstractMessage[] GetMessagesFor(IBusService service)
        {
            lock (LOCK)
            {
                var messages = _queues[service].ToArray();
                _queues[service].Clear();

                return messages;
            }
        }
    }
}
