using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using MessageServer.Interfaces;
using MessageServer.Contracts.Messages;

namespace MessageServer.Services
{
    /// <summary>
    /// An abstract base class for a service that operates on the MessageBus. Provides a lot of
    /// useful base functionality. It is recommended that any services extend this instead of 
    /// just implementing IBusService.
    /// 
    /// AbstractBusService will automatically poll the bus every 30 milliseconds for new 
    /// messages registered via SetMessageHandler.
    /// </summary>
    public abstract class AbstractBusService : IBusService
    {
        private const int BUS_POLL_TICK = 30;

        private MessageBus _messageBus;
        private Timer _pollTimer;
        private Dictionary<Type, Action<AbstractMessage>> _messageHandlers;

        /// <summary>
        /// A log4net logger for the bus service.
        /// </summary>
        protected ILog Logger { get; }

        /// <summary>
        /// A list of types of message that this service wants to be notified of.
        /// </summary>
        public List<Type> MatchingMessageTypes { get; }

        /// <summary>
        /// Construct a new AbstractBusService.
        /// </summary>
        protected AbstractBusService()
        {
            Logger = LogManager.GetLogger(GetType());
            MatchingMessageTypes = new List<Type>();
            _messageHandlers = new Dictionary<Type, Action<AbstractMessage>>();
        }

        /// <summary>
        /// Register an action to handle a type of message. This automatically adds the type
        /// of message to MatchingMessageTypes.
        /// </summary>
        /// <typeparam name="T">The type of message the handler is being registered for.</typeparam>
        /// <param name="action">The action to call when a message of type T is recieved.</param>
        protected void SetMessageHandler<T>(Action<T> action) where T : AbstractMessage
        {
            try
            {
                var messageType = typeof(T);

                if (_messageHandlers.ContainsKey(messageType))
                {
                    throw new InvalidOperationException("Attempted to register duplicate message handlers for the same type.");
                }

                MatchingMessageTypes.Add(messageType);

                _messageHandlers.Add(messageType,
                    (msg) =>
                    {
                        action(msg as T);
                    });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Start the bus service.
        /// </summary>
        /// <param name="bus">The MessageBus.</param>
        public virtual void Start(MessageBus bus)
        {
            _messageBus = bus;
            _pollTimer = new Timer(PollBus, null, BUS_POLL_TICK, BUS_POLL_TICK);
        }

        /// <summary>
        /// Stop the bus service.
        /// </summary>
        public virtual void Stop()
        {
            _pollTimer.Dispose();
        }

        private void PollBus(object state)
        {
            try
            {
                var messages = _messageBus.GetMessagesFor(this);

                foreach (var pair in _messageHandlers)
                {
                    foreach (var message in messages)
                    {
                        try
                        {
                            if (TypeUtils.TypesMatch(message.GetType(), pair.Key))
                            {
                                pair.Value(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Send a message onto the MessageBus
        /// </summary>
        /// <param name="message">The message to send.</param>
        protected void SendMessage(AbstractMessage message)
        {
            _messageBus.SendMessage(message);
        }
    }
}
