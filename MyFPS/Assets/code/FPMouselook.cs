using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMouselook : MonoBehaviour
{
    private Vector3 playerVisualAngle;
    public Transform chararterTransform;
    public float MosueSpeed;
    public Vector2 MaxminAngle;
    
    public AnimationCurve RecoliCurve;
    public Vector2 RecoilRange;

    private float currentRecoilTime;
    private Vector2 currentRecoil;
    private CameraSpring cameraSpring;

    private void Start()
    {
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    // Update is called once per frame
    void Update()
    {
       var tmp_MouseX =  Input.GetAxis("Mouse X");
       var tmp_MouseY =  Input.GetAxis("Mouse Y");
       playerVisualAngle.x -= tmp_MouseY * MosueSpeed;
       playerVisualAngle.y += tmp_MouseX * MosueSpeed;
    
       CalculateRecoilOffset();
       playerVisualAngle.x -= currentRecoil.y;
       playerVisualAngle.y += currentRecoil.x;
       
       playerVisualAngle.x = Mathf.Clamp(playerVisualAngle.x, MaxminAngle.x, MaxminAngle.y);
       
       transform.rotation = Quaternion.Euler(playerVisualAngle.x, playerVisualAngle.y, 0);
       chararterTransform.rotation = Quaternion.Euler(0, playerVisualAngle.y, 0);
    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = RecoliCurve.Evaluate(currentRecoilTime);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilFraction);
    }

    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
