namespace ServerMessages
{
    public interface IMessageReceiver
    {
        public void Subscribe();

        public void Unsubscribe();
    }
}