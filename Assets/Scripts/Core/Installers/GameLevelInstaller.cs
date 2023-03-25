using Camera;
using Dialogs;
using Inventory;
using Player;
using Zenject;

namespace Core.Installers
{
  public class GameLevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle();
      Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
      Container.Bind<InputController>().FromComponentInHierarchy().AsSingle();
      Container.Bind<DialogManager>().FromComponentInHierarchy().AsSingle();
      Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
      Container.Bind<InventoryManager>().FromComponentInHierarchy().AsSingle();
    }
  }
}