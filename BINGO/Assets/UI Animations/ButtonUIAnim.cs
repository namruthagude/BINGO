using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIAnim : MonoBehaviour
{
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        /*button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);*/
    }

    public void OnButtonClick()
    {
        this.transform.localScale = Vector3.one;
        StartCoroutine(ScaleButtonAnim());
    }

    private IEnumerator ScaleButtonAnim()
    {
        transform.DOScale(1.1f,0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(1, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
