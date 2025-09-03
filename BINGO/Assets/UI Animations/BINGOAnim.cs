using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BINGOAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 30);
        transform.DORotate(new Vector3(0,0,-30), 1.5f, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo);
    }
}
