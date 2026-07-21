using System.Collections.Generic;
using Mirror;
using ServerMessages;
using Zenject;

namespace Behaviours
{
    public class CustomNetworkManager : NetworkManager
    {
        private MessageToClients _messageToClients;
        private IEnumerable<IMessageReceiver> _receivers;

        [Inject]
        private void Initialize(MessageToClients messageToClients, IEnumerable<IMessageReceiver> receivers)
        {
            _messageToClients = messageToClients;
            _receivers = receivers;
        }
        
        #region Client
    
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            foreach (IMessageReceiver receiver in _receivers)
            {
                receiver.Subscribe();
            }
        }

        public override void OnClientDisconnect()
        {
            foreach (IMessageReceiver receiver in _receivers)
            {
                receiver.Unsubscribe();
            }
        
            base.OnClientDisconnect();
        }

        #endregion

        #region Server

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            _messageToClients.Start();
        }

        public override void OnStopServer()
        {
            _messageToClients.Stop();
            
            base.OnStopServer();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            _messageToClients.UnsubscribeClient(conn);
        
            base.OnServerDisconnect(conn);
        }

        #endregion
    }
}
