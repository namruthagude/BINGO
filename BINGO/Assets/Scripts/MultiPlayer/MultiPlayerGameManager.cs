using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class MultiPlayerGameManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static MultiPlayerGameManager instance;

    [SerializeField]
    private MultiplayerUI UI_Multiplayer;
    [SerializeField]
    private GameUI gameUI;
    [SerializeField]
    private GridController gridController;

    private List<RoomInfo> roomList = new List<RoomInfo>();
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindPublicRoom()
    {
        bool found = false;
        RoomInfo foundRoom = null;
        foreach (RoomInfo room in roomList)
        {
            if(room.PlayerCount <= 1)
            {
                //Found Room and Join
                found = true;
                foundRoom = room;
                break;
            }
        }

        if (found)
        {
            //Join to the same Room
            JoinPublicRoom(foundRoom);
        }
        else
        {
            //Create a New Room
            CreatePublicRoom();

        }

        UI_Multiplayer.ShowWaitingRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player with name" +  newPlayer + "Entered Room");

        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Start Game");
            photonView.RPC(nameof(StartGameOnBothSides), RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartGameOnBothSides()
    {
        gameUI.StartGame();
    }

    public void StrikeOffNumberMultiplayer(int row, int col, int val)
    {
        photonView.RPC(nameof(RPCStrikeOffNumber), RpcTarget.Others, row, col, val);
    }

    [PunRPC]
    private void RPCStrikeOffNumber(int row, int col, int val)
    {
        gridController.StrikeOffRPC(row, col, val);
    }

    public void AddingOtherPlayerBingoLetters()
    {
        photonView.RPC(nameof(RPCOtherPlayerBingoLetters), RpcTarget.Others);
    }

    [PunRPC]
    private void RPCOtherPlayerBingoLetters()
    {
        gridController.AddingAIBingoLetters();
    }
    public void GameLost(string result)
    {
        photonView.RPC(nameof(RPCGameLost), RpcTarget.Others, result);
    }
    [PunRPC]
   private void RPCGameLost(string result)
    {
        gridController.gameUI.OnGameOver(result);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player with name" + otherPlayer + "Left Room");
    }
    private void CreatePublicRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(GenerateRandomRoom(), roomOptions , TypedLobby.Default);
        Debug.Log("Created Room");
    }
    private void JoinPublicRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
        Debug.Log("Joined Room");
    }
    

    private string GenerateRandomRoom()
    {
        string roomName = "Room" + Random.Range(1000, 9999);
        return roomName;
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public void OnLeftLobby()
    {
        Debug.Log("Left Lobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       this.roomList = roomList;
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("Lobby stats updated");
    }
}
