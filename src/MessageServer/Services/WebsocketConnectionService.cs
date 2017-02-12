using System;
using System.Collections.Generic;
using System.Linq;
using MessageServer.Interfaces;
using MessageServer.Contracts.Messages;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperWebSocket;
using MessageServer.Contracts.Interfaces;

namespace MessageServer.Services
{
    /// <summary>
    /// A bus service that provides a websocket interface to the message bus. It will send 
    /// messages that extend AbstractOutgoingClientMessage to clients across a websocket 
    /// connection, and take messages that extend AbstractIncomingClientMessage from a websocket 
    /// connection and put them on the message bus.
    /// </summary>
    public class WebSocketConnectionService : AbstractBusService, IInitializableService
    {
        private readonly ISettingsProvider _settingsProvider;

        private WebSocketServer _socketServer;
        private Dictionary<string, Type> _messageTypes;

        /// <summary>
        /// Construct a new WebSocketConnectionService.
        /// </summary>
        /// <param name="settingsProvider">The settings provider to use.</param>
        /// <param name="messageTypeProviders">Type providers for every type of expected AbstractIncomingClientMessage.</param>
        public WebSocketConnectionService(
            ISettingsProvider settingsProvider, 
            IEnumerable<IIncomingMessageTypeProvider> messageTypeProviders)
        {
            _settingsProvider = settingsProvider;

            SetMessageHandler<AbstractOutgoingClientMessage>(HandleOutgoingMessage);

            _messageTypes = messageTypeProviders
                .SelectMany(mtp => mtp.IncomingMessageTypes())
                .Where(t => t.IsSubclassOf(typeof(AbstractIncomingClientMessage)))
                .ToDictionary(t => t.Name);

            if(!_messageTypes.Any())
            {
                Logger.Error("No message types provided, server cannot receive external messages. Register at least one IIncomingMessageTypeProvider.");
            }
        }

        /// <summary>
        /// Initialize the WebSocketConnectionService.
        /// </summary>
        public void Initialize()
        {
            _socketServer = new WebSocketServer();
            _socketServer.Setup(_settingsProvider.GetWebsocketConfiguration().Servers.First());

            _socketServer.NewMessageReceived += MessageRecieved;
            _socketServer.NewSessionConnected += NewConnection;
            _socketServer.SessionClosed += ClosedConnection;
        }

        /// <summary>
        /// Start the WebSocketConnectionService.
        /// </summary>
        /// <param name="bus">The MessageBus.</param>
        public override void Start(MessageBus bus)
        {
            _socketServer.Start();
            Logger.Info("Listening on port " + _socketServer.Config.Port);

            base.Start(bus);
        }

        /// <summary>
        /// Stop the WebSocketConnectionService.
        /// </summary>
        public override void Stop()
        {
            base.Stop();

            _socketServer.Stop();
        }

        private void NewConnection(WebSocketSession session)
        {
            SendMessage(new ClientConnectedMessage(session.SessionID));
        }

        private void ClosedConnection(WebSocketSession session, CloseReason value)
        {
            SendMessage(new ClientDisconnectedMessage(session.SessionID));
        }

        private void MessageRecieved(WebSocketSession session, string json)
        {
            try
            {
                var commaIndex = json.IndexOf(',');

                var messageType = json.Substring(0, commaIndex);
                var body = json.Substring(commaIndex + 1);

                // really noisy
                Logger.Debug("Received message: " + messageType);

                var message = Deserialize(messageType, body, session.SessionID);

                if (message != null)
                {
                    // dump incoming messages onto the bus
                    SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void HandleOutgoingMessage(AbstractOutgoingClientMessage outgoingMessage)
        {
            var messageName = outgoingMessage.GetType().Name;

            var messageBody = messageName + ","
                + JsonConvert.SerializeObject(outgoingMessage);
            
            Logger.Debug("Sending message: " + messageName);

            foreach (var id in outgoingMessage.ClientIds)
            {
                var session = _socketServer.GetSessionByID(id);

                if (session != null)
                {
                    _socketServer.GetSessionByID(id).Send(messageBody);
                }
            }
        }

        private AbstractIncomingClientMessage Deserialize(string messageType, string body, string sessionId)
        {
            if (!_messageTypes.ContainsKey(messageType))
            {
                throw new ArgumentException("Unexpected message type: " + messageType);
            }

            var message = JsonConvert.DeserializeObject(body, _messageTypes[messageType]) as AbstractIncomingClientMessage;

            message.ClientId = sessionId;

            return message;
        }
    }
}
