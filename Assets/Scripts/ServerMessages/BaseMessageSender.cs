using System;
using Mirror;
using Zenject;

namespace ServerMessages
{
    public abstract class BaseMessageSender<T> : IMessageSender, IInitializable, IDisposable
        where T : struct, NetworkMessage
    {
        private readonly MessageToClients _messageToClients;
        private T _message;

        protected BaseMessageSender(MessageToClients messageToClients, T message)
        {
            _messageToClients = messageToClients;
            _message = message;
        }

        public void Initialize() =>
            _messageToClients.Subscribed += HandleSubscription;

        public void Dispose() =>
            _messageToClients.Subscribed -= HandleSubscription;

        public void Send() =>
            _messageToClients.Send(_message);

        public void SendTo(NetworkConnectionToClient conn) =>
            _messageToClients.SendTo(conn, _message);

        public void ChangeMessage(T message) =>
            _message = message;

        protected virtual void HandleSubscription(NetworkConnectionToClient conn, ushort messageId)
        {
        }
    }
}