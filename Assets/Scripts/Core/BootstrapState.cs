using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine gameStateMachine;

    [Inject]
    public BootstrapState(GameStateMachine gameStateMachine)
    {
      this.gameStateMachine = gameStateMachine;
    }
    
    public void Enter()
    {
      AsyncOperation loader = SceneManager.LoadSceneAsync("WorldScene");
      gameStateMachine.Enter<LoadProgressState>();
    }

    public void Next()
    {
    }

    public void Exit()
    {
    }
  }
}