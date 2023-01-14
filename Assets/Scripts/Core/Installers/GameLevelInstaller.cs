using Camera;
using Zenject;

namespace Core.Installers
{
  public class GameLevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle();
    }
  }
}