using Zenject;

namespace Core.Installers
{
  public class ProjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
      Container.Bind<GameStateFactory>().AsSingle();
      Container.Bind<SceneLoader>().AsSingle();
    }
  }
}