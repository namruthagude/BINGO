using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public enum Turn
    {
        None,
        Player1,
        Player2,
        AI
    }

    public enum Team
    {
        None,
        Team1,
        Team2
    }
    public Team playerTeam;
    private Turn GameTurn = Turn.None;
   public  List<GridCell> cells;

    public List<int> list = new List<int>();
    public List<int> playerList = new List<int>();
    public List<int> aiList = new List<int>();

    public Cell[,] playerMatrix = new Cell[5, 5];
    public Cell[,] aiMatrix = new Cell[5, 5];

    [SerializeField]
    public CameraShake cameraShake;


    [SerializeField]
    public GameUI gameUI;
    private int playerBingoCount = 0;
    private int aiBingoCount = 0;
    [SerializeField]
    private List<int> availableNumbers  = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        if (RuntimeDBManager.instance.IsMultiplayer)
        {
            GeneratingPlayerList();
            InitializeAvailableList();
            GameTurn = Turn.Player1;
            if (MultiPlayerGameManager.instance.photonView.IsMine)
            {
                playerTeam = Team.Team1;
            }
            else
            {
                playerTeam = Team.Team2;
            }
        }
        else
        {
            GeneratingPlayerList();
            GeneratingAIList();
            InitializeAvailableList();
            GameTurn = Turn.Player1;
            UpdateTurn();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeAvailableList()
    {
        for (int i = 1; i <= 25; i++)
        {
            availableNumbers.Add(i);
        }
    }

    private void InitializingList()
    {
        for (int i = 1; i <= 25; i++)
        {
            list.Add(i);
        }
    }

    private void GeneratingPlayerList()
    {
        InitializingList();
        int i = 0;
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count);
            playerList.Add(list[index]);
            cells[i].SetValue(list[index]);
            i++;
            list.RemoveAt(index);
        }
        ConvertingToPlayerMatrix();
    }

    private void GeneratingAIList()
    {
        InitializingList();
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count);
            aiList.Add(list[index]);
            list.RemoveAt(index);
        }

        ConvertingToAIMatrix();
    }

    public void ConvertingToPlayerMatrix()
    {
        for(int i = 0; i< playerList.Count; i++)
        {
            int row = i / 5;
            int col = i % 5;
            cells[i].SetRow(row);
            cells[i].SetColumn(col);
            
            playerMatrix[row, col] = new Cell();
            playerMatrix[row,col].value = playerList[i];
            playerMatrix[row, col].isStrikedOff = false;
        }
    }

    public void ConvertingToAIMatrix()
    {
        for(int i=0; i< aiList.Count; i++)
        {
            int row = i / 5;
            int col = i % 5;
            aiMatrix[row, col] = new Cell();
            aiMatrix[row, col].value = aiList[i];
            aiMatrix[row, col].isStrikedOff = false;
        }
    }


    public void StrikeOffNumber(GameObject button)
    {
        if (RuntimeDBManager.instance.IsMultiplayer)
        {
            if((playerTeam == Team.Team1 && GameTurn == Turn.Player1) || (playerTeam == Team.Team2 && GameTurn == Turn.Player2))
            {
                GridCell buttonCell = button.GetComponent<GridCell>();
                int row = buttonCell.GetRow();
                int col = buttonCell.GetColumn();
                int val = buttonCell.GetValue();
                if (buttonCell.IsStrikedOff())
                {
                    return;
                }
                gameUI.SetSelectedNumber(val.ToString(), button.transform);
                buttonCell.StrikeOff(true);
                buttonCell.clickParticles.Play();
                AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.gameButtonNumbersClip[val - 1]);
                playerMatrix[row, col].isStrikedOff = true;
                CheckForPlayerBingo(row, col);
                SwitchTurn();
                StrikeOffMultiplayer(row, col, val);
            }
            

        }
        else {
            if (GameTurn != Turn.Player1)
                return;
            GridCell buttonCell = button.GetComponent<GridCell>();
            int row = buttonCell.GetRow();
            int col = buttonCell.GetColumn();
            int val = buttonCell.GetValue();
            if (buttonCell.IsStrikedOff())
            {
                return;
            }
            gameUI.SetSelectedNumber(val.ToString(), button.transform);
            buttonCell.StrikeOff(true);
            buttonCell.clickParticles.Play();
            AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.gameButtonNumbersClip[val - 1]);
            StrikeOff(row, col);
            SwitchTurn();
        }
       
    }

    private void StrikeOff(int row, int col)
    {
        if(GameTurn == Turn.Player1)
        {
            playerMatrix[row, col].isStrikedOff = true;
            CheckForPlayerBingo(row, col);
            CheckNumberToStrikeOffInAI(playerMatrix[row, col].value);
            
        }
        else if(GameTurn == Turn.Player2)
        {
            aiMatrix[row,col].isStrikedOff = true;
            CheckForAIBingo(row, col);
            Debug.Log("Value of Ai Selected number" + aiMatrix[row, col].value);
            CheckNumberToStrikeOffInPlayer(aiMatrix[row,col].value);
        }
        availableNumbers.Remove(playerMatrix[row, col].value);

    }

    public void StrikeOffMultiplayer(int row, int col, int val)
    {
        MultiPlayerGameManager.instance.StrikeOffNumberMultiplayer(row, col,val);
    }

    public void StrikeOffRPC(int row, int col, int val)
    {
        gameUI.SetSelectedNumber(val.ToString(), gameUI.aiNumberTransform);
       
        CheckNumberToStrikeOffInPlayer(val);
   

        CheckForPlayerBingo(row, col);
        //CheckNumberToStrikeOffInAI(playerMatrix[row, col].value);
        SwitchTurn();
    }

    private void CheckNumberToStrikeOffInAI(int number)
    {
        for(int i=0; i< 5; i++)
        {
            for(int j=0; j< 5; j++)
            {
                if (aiMatrix[i,j].value == number)
                {
                    aiMatrix[i,j].isStrikedOff = true;
                    
                    CheckForAIBingo(i,j);
                    break;
                }
            }
        }
    }

    private void CheckNumberToStrikeOffInPlayer(int number)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (playerMatrix[i, j].value == number)
                {
                    playerMatrix[i, j].isStrikedOff = true;
                    cells[(i*5) + j].StrikeOff(true);
                    cells[(i * 5) + j].clickParticles.Play();
                    AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.gameButtonNumbersClip[number - 1]);
                    CheckForPlayerBingo(i, j);
                    break;
                }
            }
        }
    }

    private void CheckForPlayerBingo(int row, int col)
    {
        int count = 0;
        //Checking for Row
        for (int i = 0; i < 5; i++)
        {
            if (playerMatrix[row, i].isStrikedOff)
            {
                count++;
            }
        }
        if(count == 5)
        {
            for(int i = 0; i < 5; i++)
            {
                Debug.Log("ROW BINGO");
                cells[(5 * row) + i].ShowBingoCompleted();
            }
            
            AddingPlayerBingoLetters();
            if (RuntimeDBManager.instance.IsMultiplayer)
            {
                MultiPlayerGameManager.instance.AddingOtherPlayerBingoLetters();
            }
        }
        count = 0;
        for(int i = 0;i < 5; i++)
        {
            if (playerMatrix[i, col].isStrikedOff)
            {
                count++;
            }
        }
        if (count == 5)
        {
            
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("COLUMN BINGO");
                cells[(5 *i) + col].ShowBingoCompleted();
            }
            AddingPlayerBingoLetters();
            if (RuntimeDBManager.instance.IsMultiplayer)
            {
                MultiPlayerGameManager.instance.AddingOtherPlayerBingoLetters();
            }
        }

    }

    private void AddingPlayerBingoLetters()
    {
        cameraShake.Shake(25f, 0.5f);
        gameUI.UpdatePlayerBINGO(playerBingoCount);
        playerBingoCount++;
        if (playerBingoCount == 5)
        {
            cameraShake.Shake(30f, 0.7f);
            gameUI.OnGameOver("You Won");
            AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.bingoAudioClip);
            /*if (RuntimeDBManager.instance.IsMultiplayer)
            {
                MultiPlayerGameManager.instance.GameLost("You Lost");
            }*/
        }
       
       
    }
    private void CheckForAIBingo(int row, int col)
    {
        int count = 0;

        for(int i = 0; i< 5; i++)
        {
            if (aiMatrix[row, i].isStrikedOff)
            {
                count++;
            }
        }

        if( count == 5)
        {
            AddingAIBingoLetters();
        }

        count = 0;
        for(int i=0;i< 5; i++)
        {
            if (aiMatrix[i, col].isStrikedOff)
            {
                count++;
            }
        }

        if (count == 5)
        {
           AddingAIBingoLetters();
        }
    }

    public void AddingAIBingoLetters()
    {
        gameUI.UpdateAIBINGO(aiBingoCount);
        aiBingoCount++;
        if (aiBingoCount == 5)
        {
            gameUI.OnGameOver("You Lost");
            AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.nearMissAudioClip);
        }
    }

    private void UpdateTurn()
    {
        gameUI.UpdateTurn(GameTurn.ToString());
    }

    private void SwitchTurn()
    {
        if (RuntimeDBManager.instance.IsMultiplayer)
        {
            if (GameTurn == Turn.Player1)
            {
                GameTurn = Turn.Player2;
                UpdateTurn();

               
            }
            else if (GameTurn == Turn.Player2)
            {
                GameTurn = Turn.Player1;
                UpdateTurn();
            }
        }
        else
        {
            if (GameTurn == Turn.Player1)
            {
                GameTurn = Turn.Player2;
                UpdateTurn();
                // starting AI coroutinue
                StartCoroutine(nameof(AITurnCoroutine));

            }
            else if (GameTurn == Turn.Player2)
            {
                GameTurn = Turn.Player1;
                UpdateTurn();
            }
        }
        
    }

    private IEnumerator AITurnCoroutine()
    {
        yield return new WaitForSeconds(2);

        int selectedIndex = Random.Range(0, availableNumbers.Count);
        int selectedNumber = availableNumbers[selectedIndex];

        availableNumbers.Remove(selectedNumber);
        gameUI.SetSelectedNumber(selectedNumber.ToString(), gameUI.aiNumberTransform);

        List<int> list = new List<int>();
        list = FindRowAndColumnOfAI(selectedNumber);
        AudioManager.Singleton.PlayOneShotSFX(AudioManager.Singleton.gameButtonNumbersClip[selectedNumber - 1]);
        if (list.Count > 0)
        {
            StrikeOff(list[0], list[1]);
        }

        SwitchTurn();
    }

    private List<int> FindRowAndColumnOfAI(int number)
    {
        List<int> newList = new List<int>();
        for(int i = 0; i< 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (aiMatrix[i,j].value == number)
                {
                   newList.Add(i);
                    newList.Add(j);
                    break;
                }
            }
        }
        return newList;
    }
    [System.Serializable]
    public class Cell
    {
        public int value;
        public bool isStrikedOff;
    }


}
