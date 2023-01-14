using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine gameStateMachine;
    private readonly SceneLoader sceneLoader;

    [Inject]
    public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
      this.gameStateMachine = gameStateMachine;
      this.sceneLoader = sceneLoader;
    }
    
    public void Enter()
    {
      LoadGameScene();
    }

    public void Next()
    {
      gameStateMachine.Enter<LoadProgressState>();
    }

    public void Exit(){}

    private async UniTask LoadGameScene()
    {
      await Task.Run(sceneLoader.LoadGame);
      Next();
    }
  }
}