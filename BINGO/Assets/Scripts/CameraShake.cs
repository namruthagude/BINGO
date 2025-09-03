using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    

    void Start()
    {
        //Shake(29, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(float shakeIntensity, float shakeTime)
    {
       
       transform.DOShakePosition(shakeTime, shakeIntensity);
    }

   
}
