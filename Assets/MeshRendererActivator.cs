using UnityEngine;

[ExecuteInEditMode]
public class MeshRendererActivator : MonoBehaviour
{
    private void Start()
    {
        ActivateAllMR(transform);
    }

    private void ActivateAllMR(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.enabled = true;
            }
            ActivateAllMR(parent.GetChild(i));
        }
    }
}
