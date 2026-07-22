using System;
using Mirror;
using ServerMessages;

namespace Hello
{
    internal class HelloMessageSender : BaseMessageSender<HelloMessage>
    {
        private const string Text = "Hello Client!";

        public HelloMessageSender(MessageToClients messageToClients)
            : base(messageToClients, new HelloMessage { Text = Text })
        {
        }

        protected override void HandleSubscription(NetworkConnectionToClient conn, ushort messageId)
        {
            if (messageId == NetworkMessageId<HelloMessage>.Id)
            {
                SendTo(conn);
            }
        }
    }
}