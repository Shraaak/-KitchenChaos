using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KictchenObjectSO input;
    public KictchenObjectSO output;
    public float fryingtimerMax;

}
