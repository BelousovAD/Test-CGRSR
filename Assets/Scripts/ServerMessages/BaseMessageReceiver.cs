using Mirror;

namespace ServerMessages
{
    public abstract class BaseMessageReceiver<T> : IMessageReceiver
        where T : struct, NetworkMessage
    {
        private readonly MessageFromServer _messageFromServer;

        protected BaseMessageReceiver(MessageFromServer messageFromServer) =>
            _messageFromServer = messageFromServer;

        public void Subscribe() =>
            _messageFromServer.Subscribe<T>(Handle);

        public void Unsubscribe() =>
            _messageFromServer.Unsubscribe<T>();

        public void UnsubscribeLocally() =>
            _messageFromServer.UnsubscribeLocally<T>();

        protected abstract void Handle(T message);
    }
}