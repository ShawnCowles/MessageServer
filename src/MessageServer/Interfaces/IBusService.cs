using System;
using System.Collections.Generic;

namespace MessageServer.Interfaces
{
    public interface IBusService
    {
        HashSet<Type> MatchingMessageTypes { get; }

        void Start(MessageBus bus);

        void Stop();
    }
}
