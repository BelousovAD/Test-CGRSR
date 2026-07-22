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
            NetworkClient.Send(new SubscribeRequest { TypeName = typeof(T).AssemblyQualifiedName });
        }

        public void Unsubscribe<T>()
            where T : struct, NetworkMessage
        {
            NetworkClient.Send(new UnsubscribeRequest { TypeName = typeof(T).AssemblyQualifiedName });
            NetworkClient.UnregisterHandler<T>();
        }
    }
}