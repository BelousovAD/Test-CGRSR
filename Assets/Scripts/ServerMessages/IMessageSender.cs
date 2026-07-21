using Mirror;

namespace ServerMessages
{
    public interface IMessageSender
    {
        public void Send();

        public void SendTo(NetworkConnectionToClient conn);
    }
}