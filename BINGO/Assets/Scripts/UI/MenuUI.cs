using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject dailyRewardsPanel;
    [SerializeField]
    private GameObject streakRewardPanel;
   
    // Start is called before the first frame update
    void Start()
    {
        ShowStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOffAllPanels()
    {
        startPanel.SetActive(false);
        dailyRewardsPanel.SetActive(false);
        streakRewardPanel.SetActive(false);
    }

    public void ShowStart()
    {
        TurnOffAllPanels();
        startPanel.SetActive(true);
    } 

    public void ShowDailyRewards()
    {
        TurnOffAllPanels();
        dailyRewardsPanel?.SetActive(true);
    }

    public void ShowStreakReward()
    {
        //TurnOffAllPanels();
        if (streakRewardPanel.activeSelf)
        {
            streakRewardPanel.SetActive(false );
        }
        else
        {
            streakRewardPanel?.SetActive(true);
        }
        
    }

    public void OnPlay()
    {
        LoadingScreen.Singleton.LoadScene(LoadingScreen.SCENE_GAME);
    }
}
