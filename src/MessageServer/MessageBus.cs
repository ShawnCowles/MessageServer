using System.Collections.Generic;
using MessageServer.Messages;
using MessageServer.Interfaces;
using System.Linq;
using System;

namespace MessageServer
{
    public class MessageBus
    {
        private static readonly object LOCK = new object();

        private Dictionary<IBusService, List<AbstractMessage>> _queues;
        private IBusService[] _busServices;

        public MessageBus(IEnumerable<IBusService> services)
        {
            _queues = new Dictionary<IBusService, List<AbstractMessage>>();

            _busServices = services.ToArray();

            foreach (var service in services)
            {
                _queues.Add(service, new List<AbstractMessage>());
            }
        }

        public void Start()
        {
            foreach (var service in _busServices)
            {
                service.Start(this);
            }
        }

        public void Stop()
        {
            foreach (var service in _busServices)
            {
                service.Stop();
            }
        }

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
