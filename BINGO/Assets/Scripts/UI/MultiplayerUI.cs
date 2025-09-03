using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject typeSelection;
    [SerializeField]
    private GameObject waitingRoom;
    // Start is called before the first frame update
    void Start()
    {
        ShowTypeSelection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWaitingRoom()
    {
        TurnOffAllPanels();
        waitingRoom.SetActive(true);
    }
    public void ShowTypeSelection()
    {
        TurnOffAllPanels();
        typeSelection.SetActive(true);
    }
    private void TurnOffAllPanels()
    {
        typeSelection.SetActive(false);
        waitingRoom.SetActive(false);
    }

    public void OnPublicRoomCLicked()
    {
        MultiPlayerGameManager.instance.FindPublicRoom();
    }
}
