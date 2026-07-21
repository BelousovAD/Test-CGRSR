using ServerMessages;
using Zenject;

namespace Hello
{
    internal class HelloInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessageReceiver>().To<HelloMessageReceiver>().AsSingle();
            Container.BindInterfacesAndSelfTo<HelloMessageSender>().AsSingle();
        }
    }
}