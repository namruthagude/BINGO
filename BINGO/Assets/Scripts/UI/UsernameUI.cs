using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInput;
    // Start is called before the first frame update
    void Start()
    {
        nameInput.text = GenerateUserName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string GenerateUserName()
    {
        string name = "GUEST_" + Random.Range(1000, 9999).ToString();
        return name;
    }

    public void OnOKClick()
    {
        PlayerPrefs.SetString(PlayerPrefsStrings.USERNAME, nameInput.text);
        this.gameObject.SetActive(false);
    }
}
