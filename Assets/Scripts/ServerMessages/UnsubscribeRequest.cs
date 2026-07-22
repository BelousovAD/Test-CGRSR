using Mirror;

namespace ServerMessages
{
    internal struct UnsubscribeRequest : NetworkMessage
    {
        public string TypeName;
    }
}