using Mirror;

namespace ServerMessages
{
    internal interface IMessageSender
    {
        public void Send();

        public void SendTo(NetworkConnectionToClient conn);
    }
}