using System;
using Mirror;

namespace ServerMessages
{
    public class MessageFromServer
    {
        public void Subscribe<T>(Action<T> handler)
            where T : struct, NetworkMessage
        {
            NetworkClient.RegisterHandler(handler);
            NetworkClient.Send(new SubscribeRequest { TypeId = NetworkMessageId<T>.Id });
        }

        public void Unsubscribe<T>()
            where T : struct, NetworkMessage
        {
            NetworkClient.Send(new UnsubscribeRequest { TypeId = NetworkMessageId<T>.Id });
            UnsubscribeLocally<T>();
        }

        public void UnsubscribeLocally<T>()
            where T : struct, NetworkMessage =>
            NetworkClient.UnregisterHandler<T>();
    }
}