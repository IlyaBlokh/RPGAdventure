using Zenject;

namespace Core
{
  public class GameStateFactory
  {
    private readonly IInstantiator instantiator;
    protected GameStateFactory(IInstantiator instantiator)
    {
      this.instantiator = instantiator;
    }

    public IState Create<IState>()=> instantiator.Instantiate<IState>();
  }
}