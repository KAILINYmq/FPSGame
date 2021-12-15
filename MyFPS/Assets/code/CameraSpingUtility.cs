using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpingUtility
{
    public Vector3 Values;

    private float frequence;
    private float damp;
    private Vector3 dampValues;

    public CameraSpingUtility(float _frequence, float _damp)
    {
        frequence = _frequence;
        damp = _damp;
    }

    public void UpdateSpring(float _deltalTime, Vector3 _target)
    {
        Values -= _deltalTime * frequence * dampValues;
        dampValues = Vector3.Lerp(dampValues, Values-_target, damp*_deltalTime);
    }
}