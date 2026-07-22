using Mirror;

namespace ServerMessages
{
    internal struct SubscribeRequest : NetworkMessage
    {
        public ushort TypeId;
    }
}