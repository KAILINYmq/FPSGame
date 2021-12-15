using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    public float Frequence=25;
    public float Damp=15;

    public Vector2 MinRecoilRange;
    public Vector2 MaxRecoilRange;

    private CameraSpingUtility CameraSpingUtility;
    
    void Start()
    {
        CameraSpingUtility = new CameraSpingUtility(Frequence, Damp);
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            quaternion.Euler(CameraSpingUtility.Values), Time.deltaTime*10);
    }

    void Update()
    {
        CameraSpingUtility.UpdateSpring(Time.deltaTime, Vector3.zero);
    }

    public void StartCameraSpring()
    {
        CameraSpingUtility.Values = new Vector3(0, 
            UnityEngine.Random.Range(MinRecoilRange.x, MaxRecoilRange.x),
            UnityEngine.Random.Range(MinRecoilRange.y, MaxRecoilRange.y));
    }
}
