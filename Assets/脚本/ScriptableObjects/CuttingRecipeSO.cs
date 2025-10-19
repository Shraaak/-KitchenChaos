using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KictchenObjectSO input;
    public KictchenObjectSO output;
    public int cuttingProgressMax;

}
