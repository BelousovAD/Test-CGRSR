using System;
using System.Collections.Generic;
using Mirror;

namespace ServerMessages
{
    public class MessageToClients
    {
        private readonly Dictionary<ushort, HashSet<NetworkConnectionToClient>> _listeners = new ();

        public event Action<NetworkConnectionToClient, ushort> Subscribed;

        public void StartServer()
        {
            NetworkServer.RegisterHandler<UnsubscribeRequest>(UnsubscribeClientFromMessage);
            NetworkServer.RegisterHandler<SubscribeRequest>(SubscribeClientToMessage);
        }

        public void StopServer()
        {
            NetworkServer.UnregisterHandler<SubscribeRequest>();
            NetworkServer.UnregisterHandler<UnsubscribeRequest>();

            foreach (HashSet<NetworkConnectionToClient> listeners in _listeners.Values)
            {
                listeners.Clear();
            }
            
            _listeners.Clear();
        }

        public void Send<T>(T message)
            where T : struct, NetworkMessage
        {
            if (_listeners.TryGetValue(NetworkMessageId<T>.Id, out HashSet<NetworkConnectionToClient> listeners) == false)
            {
                return;
            }
        
            foreach (NetworkConnectionToClient listener in listeners!)
            {
                listener.Send(message);
            }
        }

        public void SendTo<T>(NetworkConnectionToClient conn, T message)
            where T : struct, NetworkMessage
        {
            if (_listeners.TryGetValue(NetworkMessageId<T>.Id, out HashSet<NetworkConnectionToClient> listeners) == false)
            {
                return;
            }

            if (listeners.Contains(conn))
            {
                conn.Send(message);
            }
        }

        public void UnsubscribeClient(NetworkConnectionToClient conn)
        {
            foreach (HashSet<NetworkConnectionToClient> listeners in _listeners.Values)
            {
                listeners.Remove(conn);
            }
        }

        private void SubscribeClientToMessage(NetworkConnectionToClient conn, SubscribeRequest request)
        {
            if (_listeners.TryGetValue(request.TypeId, out HashSet<NetworkConnectionToClient> listeners))
            {
                if (listeners.Add(conn) == false)
                {
                    return;
                }
            }
            else
            {
                _listeners.Add(request.TypeId, new HashSet<NetworkConnectionToClient> { conn });
            }

            Subscribed?.Invoke(conn, request.TypeId);
        }
        
        private void UnsubscribeClientFromMessage(NetworkConnectionToClient conn, UnsubscribeRequest request)
        {
            if (_listeners.TryGetValue(request.TypeId, out HashSet<NetworkConnectionToClient> listeners))
            {
                listeners.Remove(conn);
            }
        }
    }
}