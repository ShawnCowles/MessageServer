using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using MessageServer.Messages;
using MessageServer.Interfaces;

namespace MessageServer.Services
{
    public abstract class AbstractBusService : IBusService
    {
        private const int BUS_POLL_TICK = 30;

        private MessageBus _messageBus;
        private Timer _pollTimer;
        private Dictionary<Type, Action<AbstractMessage>> _messageHandlers;
        protected ILog Logger { get; }

        public HashSet<Type> MatchingMessageTypes { get; }

        public AbstractBusService()
        {
            Logger = LogManager.GetLogger(GetType());
            MatchingMessageTypes = new HashSet<Type>();
            _messageHandlers = new Dictionary<Type, Action<AbstractMessage>>();
        }

        public void SetMessageHandler<T>(Action<T> action) where T : AbstractMessage
        {
            try
            {
                // Need to account for subclasses, so expand to include all possible subclasses,
                // and include the original type.
                // This may be slow, but only needs to be done at startup.
                var allTypes = typeof(T).Assembly
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(T)))
                    .Concat(new[] { typeof(T) })
                    .ToArray();

                foreach (var messageType in allTypes)
                {
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public virtual void Start(MessageBus bus)
        {
            _messageBus = bus;
            _pollTimer = new Timer(PollBus, null, BUS_POLL_TICK, BUS_POLL_TICK);
        }

        public virtual void Stop()
        {
            _pollTimer.Dispose();
        }

        private void PollBus(object state)
        {
            try
            {
                var messages = _messageBus.GetMessagesFor(this);

                foreach (var message in messages)
                {
                    _messageHandlers[message.GetType()](message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected void SendMessage(AbstractMessage message)
        {
            _messageBus.SendMessage(message);
        }
    }
}
