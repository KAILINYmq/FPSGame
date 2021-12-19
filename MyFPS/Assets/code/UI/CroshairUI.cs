using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroshairUI : MonoBehaviour
{
    public RectTransform Reticle;
    public CharacterController CharacterController;
    
    public float OriginalSize;
    public float TargetSize;
    public bool IsFire; 

    private float currentSize;

    void Update()
    {
        if (CharacterController.velocity.magnitude > 0 || IsFire)
        {
            currentSize = Mathf.Lerp(currentSize, TargetSize, Time.deltaTime*5);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, OriginalSize, Time.deltaTime*5);
        }

        Reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }
}