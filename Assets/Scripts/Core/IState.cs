namespace Core
{
  public interface IState
  {
    void Enter();
    void Next();
    void Exit();
  }
}