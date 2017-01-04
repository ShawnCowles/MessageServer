using System;
using System.Collections.Generic;
using System.Linq;
using MessageServer.Messages;
using MessageServer.Interfaces;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperWebSocket;

namespace MessageServer.Services
{
    public class WebSocketConnectionService : AbstractBusService, IInitializableService
    {
        private readonly ISettingsProvider _settingsProvider;

        private WebSocketServer _socketServer;
        private Dictionary<string, Type> _messageTypes;

        public WebSocketConnectionService(ISettingsProvider settingsProvider, IEnumerable<IIncomingMessageTypeProvider> messageTypeProviders)
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

        public void Initialize()
        {
            _socketServer = new WebSocketServer();
            _socketServer.Setup(_settingsProvider.GetWebsocketConfiguration().Servers.First());

            _socketServer.NewMessageReceived += MessageRecieved;
            _socketServer.NewSessionConnected += NewConnection;
            _socketServer.SessionClosed += ClosedConnection;
        }

        public override void Start(MessageBus bus)
        {
            _socketServer.Start();
            Logger.Info("Listening on port " + _socketServer.Config.Port);

            base.Start(bus);
        }

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
            var messageBody = outgoingMessage.MessageName + ","
                + JsonConvert.SerializeObject(outgoingMessage);
            
            Logger.Debug("Sending message: " + outgoingMessage.MessageName);

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
