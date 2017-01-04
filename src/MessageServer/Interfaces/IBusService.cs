using System;
using System.Collections.Generic;

namespace MessageServer.Interfaces
{
    public interface IBusService
    {
        List<Type> MatchingMessageTypes { get; }

        void Start(MessageBus bus);

        void Stop();
    }
}
