using ServerMessages;
using UnityEngine;

namespace Hello
{
    internal class HelloMessageReceiver : BaseMessageReceiver<HelloMessage>
    {
        public HelloMessageReceiver(MessageFromServer messageFromServer)
            : base(messageFromServer)
        {
        }

        protected override void Handle(HelloMessage message) =>
            Debug.Log(message.Text);
    }
}