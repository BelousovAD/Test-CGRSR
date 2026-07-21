using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ServerMessages
{
    public class MessageToClients : IDisposable
    {
        private readonly Dictionary<Type, HashSet<NetworkConnectionToClient>> _listeners = new ();

        public MessageToClients() =>
            NetworkServer.RegisterHandler<SubscribeRequest>(SubscribeClientToMessage);

        public event Action<NetworkConnectionToClient, Type> Subscribed;

        public void Dispose()
        {
            NetworkServer.UnregisterHandler<SubscribeRequest>();

            foreach (HashSet<NetworkConnectionToClient> listeners in _listeners.Values)
            {
                listeners.Clear();
            }
            
            _listeners.Clear();
        }

        public void Send<T>(T message)
            where T : struct, NetworkMessage
        {
            if (_listeners.TryGetValue(typeof(T), out HashSet<NetworkConnectionToClient> listeners) == false)
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
            if (_listeners.TryGetValue(typeof(T), out HashSet<NetworkConnectionToClient> listeners) == false)
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
            Type type = Type.GetType(request.TypeName);

            if (type is null)
            {
                Debug.LogError($"Failed to subscribe client {conn.connectionId}" +
                               $" to message type {request.TypeName}. Type not found.");
                return;
            }
            
            if (_listeners.TryGetValue(type, out HashSet<NetworkConnectionToClient> listeners))
            {
                listeners.Add(conn);
            }
            else
            {
                _listeners.Add(type, new HashSet<NetworkConnectionToClient> { conn });
            }

            Subscribed?.Invoke(conn, type);
        }
    }
}