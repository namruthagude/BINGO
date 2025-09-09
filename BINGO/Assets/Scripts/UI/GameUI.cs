using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    [SerializeField]
    private TMP_Text player1Name;
    [SerializeField] 
    private TMP_Text player2Name;
    [SerializeField]
    private List<Image> player1BingoLetters;
    [SerializeField]
    private List<Image> player2BingoLetters;

    [SerializeField]
    private Button button_yourTurn;
    [SerializeField]
    private Button button_oppTurn;
    [SerializeField]
    private TMP_Text selectedNumber;
    [SerializeField]
    private Transform selectedNumberTransform;
    [SerializeField]
    public Transform aiNumberTransform;

    [SerializeField]
    private GameOverUI gameOverUI;
    [SerializeField]
    private GameObject selectedNumberGO;
    [SerializeField]
    private GameObject game;
    [SerializeField]
    private GameObject start;

    [SerializeField]
    private GameObject go_Start;
    [SerializeField]
    private GameObject go_Game;
    [SerializeField]
    private GameObject go_MultiPlayer;
    [SerializeField]
    private GameObject go_Finish;
    [SerializeField]
    private GameObject go_UserName;
    [SerializeField]
    private GameObject go_Settings;
    [SerializeField]
    private GameObject go_Tutorial;
    [SerializeField]
    private Color[] BINGOColors;
    // Start is called before the first frame update
    void Start()
    {
        button_oppTurn.gameObject.SetActive(false);
        button_yourTurn.gameObject.SetActive(false);
        ShowStart();
        RuntimeDBManager.instance.OnNoUserName += Player_OnNoUserName;
    }

    private void Player_OnNoUserName()
    {
        go_UserName.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOffAllPanels()
    {
        go_Finish.SetActive(false);
        go_Game.SetActive(false);
        go_MultiPlayer.SetActive(false);
        go_Start.SetActive(false);
        go_UserName.SetActive(false);
        go_Settings.SetActive(false);
        go_Tutorial.SetActive(false);
    }
    public void UpdateTurn(GridController.Turn gameTurn)
    {
        if (RuntimeDBManager.instance.IsMultiplayer)
        {

        }
        else
        {
            if(gameTurn == GridController.Turn.Player1)
            {
                button_yourTurn.gameObject.SetActive(true);
                button_oppTurn.gameObject.SetActive(false);
            }
            else
            {
                button_yourTurn.gameObject.SetActive(false);
                button_oppTurn.gameObject.SetActive(true);
            }
        }
    }

    public void Shake()
    {
        
    }
    public void SetSelectedNumber(string value, Transform startPos)
    {
        selectedNumber.text = value;
        StopCoroutine(nameof(SelectNumberCoroutinue));
        StartCoroutine(SelectNumberCoroutinue(startPos));
    }

    private IEnumerator SelectNumberCoroutinue(Transform startPos)
    {
       
        selectedNumberGO.transform.position = startPos.position;
        selectedNumberGO.SetActive(true);
        selectedNumberGO.transform.DOMove(selectedNumberTransform.position, 1f);
        yield return new WaitForSeconds(5);
        selectedNumberGO.SetActive(false);
    }
    public void UpdatePlayerBINGO(int i)
    {
        player1BingoLetters[i].GetComponent<BINGOAnim>().PlayAnimation();
        BINGOColors[i].a = 1f;
        TMP_Text text = player1BingoLetters[i].GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.color = Color.white;
        }
        player1BingoLetters[i].color = BINGOColors[i];
    }

    public void UpdateAIBINGO(int i)
    {
        TMP_Text text = player2BingoLetters[i].GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.color = Color.white;
        }
        BINGOColors[i].a = 1f;
        player2BingoLetters[i].color = BINGOColors[i];
        //player2BingoLetters[i].color = Color.white;
    }

    public void OnGameOver(string gameResult)
    {
        TurnOffAllPanels();
        go_Finish.SetActive(true);
        gameOverUI.SetText(gameResult);


    }

    public void StartGame()
    {
        /*if (PlayerPrefs.HasKey(PlayerPrefsStrings.HASPLAYEDTUTORIAL))
        {
            if(PlayerPrefs.GetInt(PlayerPrefsStrings.HASPLAYEDTUTORIAL) == 1)
            {
                TurnOffAllPanels();
                go_Game.SetActive(true);
            }
            else
            {
                ShowTutorial();
            }
        }
        else
        {
            ShowTutorial();
        }*/

        TurnOffAllPanels();
        go_Game.SetActive(true);
    }

    public void ShowStart()
    {
        TurnOffAllPanels();
        go_Start.SetActive(true );
    }
    public void ShowMultiplayerUI()
    {
        TurnOffAllPanels();
        RuntimeDBManager.instance.IsMultiplayer = true;
        go_MultiPlayer.SetActive(true);
    }

    public void ShowSettings()
    {
        go_Settings.SetActive(true);
    }

    public void HideSettings()
    {
        go_Settings.SetActive(false);
    }

    public void ShowTutorial()
    {
        TurnOffAllPanels();
        go_Tutorial.SetActive(true);
        PlayerPrefs.SetInt(PlayerPrefsStrings.HASPLAYEDTUTORIAL, 1);
    }

    public void HideTutorial()
    {
        go_Tutorial.SetActive(false);
        go_Game.SetActive(true);
    }

}
