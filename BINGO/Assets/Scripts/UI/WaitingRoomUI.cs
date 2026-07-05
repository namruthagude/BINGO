using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingRoomUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text_playerName;
    [SerializeField]
    private GameObject go_waitingForOtherPlayer;
    // Start is called before the first frame update
    void Start()
    {
        text_playerName.text = RuntimeDBManager.instance.UserName;
        go_waitingForOtherPlayer.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
