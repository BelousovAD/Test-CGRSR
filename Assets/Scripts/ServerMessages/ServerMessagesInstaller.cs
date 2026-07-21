using Zenject;

namespace ServerMessages
{
    internal class ServerMessagesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MessageToClients>().AsSingle();
            Container.Bind<MessageFromServer>().AsSingle();
        }
    }
}