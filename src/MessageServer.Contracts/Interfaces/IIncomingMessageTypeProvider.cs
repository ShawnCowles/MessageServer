using System;
using System.Collections.Generic;

namespace MessageServer.Contracts.Interfaces
{
    public interface IIncomingMessageTypeProvider
    {
        IEnumerable<Type> IncomingMessageTypes();
    }
}
