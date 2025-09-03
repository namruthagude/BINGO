using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Step1;
    [SerializeField]
    private GameObject go_Step2;
    [SerializeField]
    private GameObject go_Step3;
    [SerializeField]
    private GameObject go_Step4;
    [SerializeField]
    private GameObject go_Step5;
    [SerializeField]
    private GameObject go_Step6;
    [SerializeField]
    private TMP_Text text_Step1;
    [SerializeField]
    private TMP_Text text_Step4;
    [SerializeField]
    private TMP_Text text_Step5;
    [SerializeField]
    private TMP_Text text_Step6;
    [SerializeField]
    private GameObject goText_Step4;
    [SerializeField]
    private GameObject goText_Step5;
    [SerializeField]
    private GameObject goText_Step6;
    // Start is called before the first frame update
    void Start()
    {
        ShowStep1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOffAllPanels()
    {
        go_Step1.SetActive(false);
        go_Step2.SetActive(false);
        go_Step3.SetActive(false);
        go_Step4.SetActive(false);
        go_Step5.SetActive(false);
        go_Step6.SetActive(false);
    }

    public void ShowStep1()
    {
        TurnOffAllPanels();
        go_Step1.SetActive(true);
    }
    public void ShowStep2()
    {
        TurnOffAllPanels();
        go_Step2.SetActive(true);
    }
    public void ShowStep3()
    {
        TurnOffAllPanels();
        go_Step3.SetActive(true);
    }

    public void ShowStep4()
    {
        TurnOffAllPanels();
        go_Step4.SetActive(true);
    }

    public void ShowStep5()
    {
        TurnOffAllPanels();
        go_Step5.SetActive(true);
    }

    public void ShowStep6()
    {
        TurnOffAllPanels();
        go_Step6.SetActive(true);
    }


    public void OnStep1ButtonClicked(Button button)
    {
        Image image = button.GetComponent<Image>();

        image.color = Color.blue;
       text_Step1.text = "Click On the Next Button";
    }

    public void OnStep4ButtonClicked(Button button)
    {
        Image image = button.GetComponent<Image>();
        image.color = Color.blue;
        text_Step4.text = "Click On the Next Button";
        goText_Step4.SetActive(true);
    }

    public void OnStep5ButtonClicked(Button button)
    {
        Image image = button.GetComponent<Image>();
        image.color = Color.blue;
        text_Step5.text = "Click On the Next Button";
        goText_Step5.SetActive(true);
    }

    public void OnStep6ButtonClicked(Button button)
    {
        Image image = button.GetComponent<Image>();
        image.color = Color.blue;
        text_Step6.text = "Click On the Skip Button";
        goText_Step6.SetActive(true);
    }
}
