using Player;
using UnityEngine;
using Zenject;

namespace Interaction
{
  public interface IInteractable
  {
    void Interact();
  }

  [RequireComponent(typeof(Collider))]
  public abstract class Interactable : MonoBehaviour, IInteractable
  {
    [SerializeField] private Collider interactBounds;
    private InputController inputController;

    [Inject]
    private void Construct(InputController inputController)
    {
      this.inputController = inputController;
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player")) 
        EnableInteraction();
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Player"))
        DisableInteraction();
    }

    private void EnableInteraction() => 
      inputController.CurrentInteractable = this;

    protected virtual void DisableInteraction() => 
      inputController.CurrentInteractable = null;
    public abstract void Interact();
  }
}