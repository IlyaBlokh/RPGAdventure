using Zenject;

namespace Core
{
  public class ProjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
      Container.Bind<GameStateFactory>().AsSingle();
    }
  }
}