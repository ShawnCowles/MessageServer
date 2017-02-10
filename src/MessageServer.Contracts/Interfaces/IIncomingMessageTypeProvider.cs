using System;
using System.Collections.Generic;

namespace MessageServer.Contracts.Interfaces
{
    /// <summary>
    /// A provider of types of possible incoming messages. This provides the list of possible 
    /// messages for WebsocketConnectionService to consider when deserializing incoming 
    /// messages.
    /// </summary>
    public interface IIncomingMessageTypeProvider
    {
        /// <summary>
        /// Get an Enumerable of possible incoming messages.
        /// </summary>
        /// <returns>An Enumerable of possible incoming messages.</returns>
        IEnumerable<Type> IncomingMessageTypes();
    }
}
