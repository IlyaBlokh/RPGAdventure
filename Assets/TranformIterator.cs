using UnityEngine;

public class TranformIterator : MonoBehaviour
{
    void Start()
    {
        //sample
        IterateOverChild(transform, 0, 2);   
    }

    private void IterateOverChild(Transform original, int currentLevel, int maxLevel)
    {
        if (currentLevel > maxLevel) return;
        for (var i = 0; i < original.childCount; i++)
        {
            Debug.Log($"{original.GetChild(i)}"); 
            IterateOverChild(original.GetChild(i), currentLevel + 1, maxLevel);
        }
    }
}
