using System;
using System.Collections.Generic;

namespace MessageServer.Interfaces
{
    public interface IIncomingMessageTypeProvider
    {
        IEnumerable<Type> IncomingMessageTypes();
    }
}
