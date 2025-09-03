using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialButtonUIAnim : MonoBehaviour
{
    [SerializeField]
    private float scaleValue;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(scaleValue, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
