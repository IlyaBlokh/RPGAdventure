using Dialogs;
using Zenject;

namespace Interaction
{
  public class DialogInteractable : Interactable
  {
    private DialogManager dialogManager;

    [Inject]
    private void Construct(DialogManager dialogManager)
    {
      this.dialogManager = dialogManager;
    }
    public override void Interact() => 
      dialogManager.StartDialogWith(this);

    protected override void DisableInteraction() => 
      dialogManager.StopDialog();
  }
}