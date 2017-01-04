using System;
using System.Collections.Generic;
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

        public List<Type> MatchingMessageTypes { get; }

        public AbstractBusService()
        {
            Logger = LogManager.GetLogger(GetType());
            MatchingMessageTypes = new List<Type>();
            _messageHandlers = new Dictionary<Type, Action<AbstractMessage>>();
        }

        public void SetMessageHandler<T>(Action<T> action) where T : AbstractMessage
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

                foreach (var pair in _messageHandlers)
                {
                    foreach (var message in messages)
                    {
                        if (TypeUtils.TypesMatch(message.GetType(), pair.Key))
                        {
                            pair.Value(message);
                        }
                    }
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
