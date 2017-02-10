using System;
using System.Collections.Generic;

namespace MessageServer.Interfaces
{
    /// <summary>
    /// Any service that can operate on the MessageBus.
    /// </summary>
    public interface IBusService
    {
        /// <summary>
        /// A list of types of message that this service wants to be notified of.
        /// </summary>
        List<Type> MatchingMessageTypes { get; }

        /// <summary>
        /// Start the bus service.
        /// </summary>
        /// <param name="bus">The MessageBus.</param>
        void Start(MessageBus bus);

        /// <summary>
        /// Stop the bus service.
        /// </summary>
        void Stop();
    }
}
