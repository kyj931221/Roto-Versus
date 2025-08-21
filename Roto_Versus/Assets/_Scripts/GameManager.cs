using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode { Solo_AI, Local_Multiplayer }
    [Header("Game Settings")]
    public GameMode currentGameMode = GameMode.Local_Multiplayer;

    [Header("Component Links")]
    public PlayerController player1Controller;
    public PlayerController player2Controller;
    public UIManager uiManager;
    public BoardManager boardManager;

    public enum GameState { RPS_Input, RPS_Resolution, WinnerChoice, TurnStart, WaitingForInput, TurnResolution, GameOver }
    private GameState currentState;

    private int turnNumber = 1;
    private bool isPlayer1Mover = true;
    private bool? winnerChoseFirst = null;

    private MoveDirection? moveChoice = null;
    private RotateDirection? rotateChoice = null;
    private RPSChoice? p1_rpsChoice = null;
    private RPSChoice? p2_rpsChoice = null;
    private Vector2Int player1GridPos;
    private Vector2Int player2GridPos;

    void Start()
    {
        if (uiManager == null || boardManager == null || player1Controller == null || player2Controller == null)
        {
            Debug.LogError("Required components are not assigned in the GameManager Inspector!");
            return;
        }
        InitializePlayerPositions();
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        while (true)
        {
            currentState = GameState.RPS_Input;
            p1_rpsChoice = null;
            p2_rpsChoice = null;
            uiManager.ShowRPSUI();
            yield return new WaitUntil(() => p1_rpsChoice.HasValue && p2_rpsChoice.HasValue);

            currentState = GameState.RPS_Resolution;
            int winner = DetermineRPSWinner();

            if (winner == 1 || winner == 2)
            {
                uiManager.ShowRPSResult(winner == 1 ? "Player 1 Wins!" : "Player 2 Wins!");
                yield return new WaitForSeconds(1.5f);

                currentState = GameState.WinnerChoice;
                winnerChoseFirst = null;
                uiManager.ShowWinnerChoiceUI(winner);
                yield return new WaitUntil(() => winnerChoseFirst.HasValue);

                if (winner == 1) isPlayer1Mover = winnerChoseFirst.Value;
                else isPlayer1Mover = !winnerChoseFirst.Value;
                break;
            }
            else
            {
                uiManager.ShowRPSResult("Draw! Again...");
                yield return new WaitForSeconds(1.5f);
            }
        }

        uiManager.HideAllSetupUI();
        Debug.Log("Setup Finished! Player 1 will be mover: " + isPlayer1Mover);

        // 게임 루프 시작 직전에 보드를 생성합니다.
        boardManager.CreateBoard();

        // 보드를 랜덤한 각도로 회전시킵니다.
        int randomRotations = Random.Range(0, 4); // 0, 1, 2, 3 중 하나를 랜덤 선택
        float targetAngle = randomRotations * 90f;  // 0, 90, 180, 270도 중 하나
        boardManager.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        Debug.Log("Board randomly rotated to: " + targetAngle + " degrees");

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (currentState != GameState.GameOver)
        {
            yield return StartCoroutine(TurnRoutine());
        }
        Debug.Log("Game Over!");
    }

    IEnumerator TurnRoutine()
    {
        currentState = GameState.TurnStart;
        Debug.Log("--- Turn " + turnNumber + " Start ---");

        moveChoice = null;
        rotateChoice = null;

        int moverNum = isPlayer1Mover ? 1 : 2;
        int rotatorNum = isPlayer1Mover ? 2 : 1;

        // --- 1. Mover 선택 단계 ---
        currentState = GameState.WaitingForInput;
        yield return StartCoroutine(uiManager.AnimateMapTransition(rotatorNum));
        uiManager.ShowPlayerUI(moverNum, true);
        yield return new WaitUntil(() => moveChoice.HasValue);
        uiManager.HidePlayerUI(moverNum);

        // --- 2. Rotator 선택 단계 ---
        currentState = GameState.WaitingForInput;
        yield return StartCoroutine(uiManager.AnimateMapTransition(moverNum));
        uiManager.ShowPlayerUI(rotatorNum, false);
        yield return new WaitUntil(() => rotateChoice.HasValue);
        uiManager.HidePlayerUI(rotatorNum);

        // --- 3. 턴 실행 단계 ---
        currentState = GameState.TurnResolution;
        yield return StartCoroutine(uiManager.AnimateMapTransition(0));

        yield return StartCoroutine(boardManager.RotateBoardRoutine(rotateChoice.Value));
        Vector2Int moveVector = GetMoveVector(moveChoice.Value);
        Vector2Int rotatedMoveVector = GetRotatedVector(moveVector);

        // --- 로직 변경: Mover만 이동하도록 수정 ---
        if (isPlayer1Mover)
        {
            // 1P가 Mover일 경우, 1P의 위치만 업데이트
            player1GridPos += rotatedMoveVector;
        }
        else
        {
            // 2P가 Mover일 경우, 2P의 위치만 업데이트
            player2GridPos += rotatedMoveVector;
        }
        // --- 로직 변경 끝 ---

        ClampPlayerPositions();

        Vector3 p1TargetWorldPos = new Vector3(player1GridPos.x, 0.51f, player1GridPos.y);
        Vector3 p2TargetWorldPos = new Vector3(player2GridPos.x, 0.51f, player2GridPos.y);

        // --- 로직 변경: Mover만 이동 애니메이션 실행 ---
        if (isPlayer1Mover)
        {
            yield return StartCoroutine(boardManager.player1Instance.GetComponent<PlayerPawn>().MoveRoutine(p1TargetWorldPos));
        }
        else
        {
            yield return StartCoroutine(boardManager.player2Instance.GetComponent<PlayerPawn>().MoveRoutine(p2TargetWorldPos));
        }
        // --- 로직 변경 끝 ---

        if (CheckForWinCondition()) { yield break; }

        isPlayer1Mover = !isPlayer1Mover;
        turnNumber++;
    }


    public void ReceiveRPSChoice(int playerNum, int choice)
    {
        if (currentState != GameState.RPS_Input) return;
        RPSChoice rpsChoice = (RPSChoice)choice;
        if (playerNum == 1 && !p1_rpsChoice.HasValue)
        {
            p1_rpsChoice = rpsChoice;
            uiManager.UpdateRPSStatus(1, "선택 완료!");
        }
        else if (playerNum == 2 && !p2_rpsChoice.HasValue)
        {
            p2_rpsChoice = rpsChoice;
            uiManager.UpdateRPSStatus(2, "선택 완료!");
        }
    }

    public void ReceiveWinnerChoice(bool choseFirst)
    {
        if (currentState == GameState.WinnerChoice) winnerChoseFirst = choseFirst;
    }

    public void ReceiveMoveChoice(int direction)
    {
        if (currentState == GameState.WaitingForInput) moveChoice = (MoveDirection)direction;
    }

    public void ReceiveRotateChoice(int direction)
    {
        if (currentState == GameState.WaitingForInput) rotateChoice = (RotateDirection)direction;
    }

    void InitializePlayerPositions()
    {
        int edge = (boardManager.boardSize - 1) / 2;
        player1GridPos = new Vector2Int(-edge, -edge);
        player2GridPos = new Vector2Int(edge, edge);
    }

    private int DetermineRPSWinner()
    {
        if (p1_rpsChoice == p2_rpsChoice) return 0;
        switch (p1_rpsChoice)
        {
            case RPSChoice.Rock: return (p2_rpsChoice == RPSChoice.Scissors) ? 1 : 2;
            case RPSChoice.Paper: return (p2_rpsChoice == RPSChoice.Rock) ? 1 : 2;
            case RPSChoice.Scissors: return (p2_rpsChoice == RPSChoice.Paper) ? 1 : 2;
        }
        return 0;
    }

    private Vector2Int GetMoveVector(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up: return new Vector2Int(0, 1);
            case MoveDirection.Down: return new Vector2Int(0, -1);
            case MoveDirection.Left: return new Vector2Int(-1, 0);
            case MoveDirection.Right: return new Vector2Int(1, 0);
            default: return Vector2Int.zero;
        }
    }

    private Vector2Int GetRotatedVector(Vector2Int vector)
    {
        int boardRotationTurns = Mathf.RoundToInt(boardManager.transform.eulerAngles.y / 90f) % 4;
        for (int i = 0; i < boardRotationTurns; i++) vector = new Vector2Int(vector.y, -vector.x);
        return vector;
    }

    void ClampPlayerPositions()
    {
        int maxCoord = (boardManager.boardSize - 1) / 2;
        player1GridPos.x = Mathf.Clamp(player1GridPos.x, -maxCoord, maxCoord);
        player1GridPos.y = Mathf.Clamp(player1GridPos.y, -maxCoord, maxCoord);
        player2GridPos.x = Mathf.Clamp(player2GridPos.x, -maxCoord, maxCoord);
        player2GridPos.y = Mathf.Clamp(player2GridPos.y, -maxCoord, maxCoord);
    }

    bool CheckForWinCondition()
    {
        Vector2Int goalGridPos = Vector2Int.zero;
        bool p1Wins = (player1GridPos == goalGridPos);
        bool p2Wins = (player2GridPos == goalGridPos);

        if (p1Wins || p2Wins)
        {
            currentState = GameState.GameOver;
            string message = (p1Wins && p2Wins) ? "Draw!" : (p1Wins ? "Player 1 Wins!" : "Player 2 Wins!");
            uiManager.ShowGameOverScreen(message);
            return true;
        }
        return false;
    }
}