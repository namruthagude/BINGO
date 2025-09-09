using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private TMP_Text number;
    [SerializeField]
    private GameObject bingoCompletedGO;
    [SerializeField]
    private GameObject selectedNumberGO;
    public int row;
    public int column;
    public ParticleSystem clickParticles;

    private bool isStrikedOff;
    private int value;
    private ButtonUIAnim anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<ButtonUIAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StrikeOff(bool strikeOff)
    {

        isStrikedOff = strikeOff;
        ShowSelectedNumber();
        anim.OnButtonClick();
    }

    public bool IsStrikedOff()
    {
        return isStrikedOff;
    }

    public void SetValue(int value)
    {
        this.value = value;
        number.text = value.ToString();
    }

    public int GetValue()
    {
        return this.value;
    }

    public void SetRow(int row)
    {
        this.row = row;
    }
    public int GetRow()
    {
        return this.row;
    }

    public void SetColumn(int column)
    {
        this.column = column;
    }
    public int GetColumn()
    {
        return this.column;
    }

    public void ShowBingoCompleted()
    {
        bingoCompletedGO.SetActive(true);
    }

    public void ShowSelectedNumber()
    {
        selectedNumberGO.SetActive(true);
    }
}
