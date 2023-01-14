using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
  public sealed class GameStateMachine : IInitializable
  {
    private readonly GameStateFactory stateFactory;
    private Dictionary<Type, IState> states;
    private IState activeState;

    public GameStateMachine(GameStateFactory stateFactory)
    {
      this.stateFactory = stateFactory;
    }

    public void Initialize()
    {
      states = new Dictionary<Type, IState>
      {
        [typeof(BootstrapState)] = stateFactory.Create<BootstrapState>(),
        [typeof(LoadProgressState)] = stateFactory.Create<LoadProgressState>(),
        [typeof(PlayLevelState)] = stateFactory.Create<PlayLevelState>()
      };
      Enter<BootstrapState>();
    }

    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      Debug.Log($"GSM Enter {state}");
      state.Enter();
    }

    private TState ChangeState<TState>() where TState : class, IState
    {
      activeState?.Exit();
      
      TState state = GetState<TState>();
      activeState = state;
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IState => states[typeof(TState)] as TState;
  }
}