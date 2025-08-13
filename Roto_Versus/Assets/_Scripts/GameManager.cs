// GameManager.cs (최종 업그레이드 버전)
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player1Controller;
    public PlayerController player2Controller;
    public UIManager uiManager;
    public BoardManager boardManager; // BoardManager와 통신하기 위한 연결고리

    public enum GameState { TurnStart, WaitingForInput, TurnResolution, GameOver }
    private GameState currentState;

    private int turnNumber = 1;
    private bool isPlayer1Mover = true;

    // 플레이어의 선택과 논리적 위치를 저장할 변수
    private MoveDirection? moveChoice = null;
    private RotateDirection? rotateChoice = null;
    private Vector2Int player1GridPos;
    private Vector2Int player2GridPos;

    void Start()
    {
        if (uiManager == null || boardManager == null) Debug.LogError("Required components are not assigned!");
        InitializePlayerPositions();
        StartCoroutine(GameLoop());
    }

    // 플레이어의 초기 위치를 설정하는 함수
    void InitializePlayerPositions()
    {
        int edge = (boardManager.boardSize - 1) / 2;
        player1GridPos = new Vector2Int(-edge, -edge);
        player2GridPos = new Vector2Int(edge, edge);
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

        PlayerController mover = isPlayer1Mover ? player1Controller : player2Controller;
        PlayerController rotator = isPlayer1Mover ? player2Controller : player1Controller;

        // --- 1. 이동 선택 ---
        currentState = GameState.WaitingForInput;
        Debug.Log(mover.name + "'s turn to choose a MOVE.");
        uiManager.ShowMoveCards();
        while (moveChoice == null) { yield return null; }

        // --- 2. 회전 선택 ---
        currentState = GameState.WaitingForInput;
        Debug.Log(rotator.name + "'s turn to choose a ROTATION.");
        uiManager.ShowRotateCards();
        while (rotateChoice == null) { yield return null; }

        uiManager.HideAllCards();

        // --- 3. 턴 실행! ---
        currentState = GameState.TurnResolution;
        Debug.Log("Resolving Turn! Move: " + moveChoice.Value + ", Rotate: " + rotateChoice.Value);

        yield return StartCoroutine(boardManager.RotateBoardRoutine(rotateChoice.Value));

        Vector2Int moveVector;
        switch (moveChoice.Value)
        {
            case MoveDirection.Up: moveVector = new Vector2Int(0, 1); break;
            case MoveDirection.Down: moveVector = new Vector2Int(0, -1); break;
            case MoveDirection.Left: moveVector = new Vector2Int(-1, 0); break;
            case MoveDirection.Right: moveVector = new Vector2Int(1, 0); break;
            default: moveVector = Vector2Int.zero; break;
        }

        int boardRotationTurns = Mathf.RoundToInt(boardManager.transform.eulerAngles.y / 90f);
        for (int i = 0; i < boardRotationTurns; i++)
        {
            moveVector = new Vector2Int(moveVector.y, -moveVector.x);
        }

        player1GridPos += moveVector;
        player2GridPos += moveVector;

        // --- 경계 체크 로직 추가 ---
        // 보드 가장자리 좌표 계산 (예: 5x5 보드일 경우 maxCoord = 2)
        int maxCoord = (boardManager.boardSize - 1) / 2;

        // Clamp 함수를 이용해 플레이어의 x, y 좌표가 (-maxCoord ~ maxCoord) 범위를 벗어나지 않도록 함
        player1GridPos.x = Mathf.Clamp(player1GridPos.x, -maxCoord, maxCoord);
        player1GridPos.y = Mathf.Clamp(player1GridPos.y, -maxCoord, maxCoord);

        player2GridPos.x = Mathf.Clamp(player2GridPos.x, -maxCoord, maxCoord);
        player2GridPos.y = Mathf.Clamp(player2GridPos.y, -maxCoord, maxCoord);
        // --- 경계 체크 로직 끝 ---

        Vector3 p1TargetWorldPos = new Vector3(player1GridPos.x, 0.51f, player1GridPos.y);
        Vector3 p2TargetWorldPos = new Vector3(player2GridPos.x, 0.51f, player2GridPos.y);

        yield return StartCoroutine(boardManager.player1Instance.GetComponent<PlayerPawn>().MoveRoutine(p1TargetWorldPos));
        yield return StartCoroutine(boardManager.player2Instance.GetComponent<PlayerPawn>().MoveRoutine(p2TargetWorldPos));

        // TODO: 승리 조건 체크 로직 추가 필요

        isPlayer1Mover = !isPlayer1Mover;
        turnNumber++;
    }

    public void ReceiveMoveChoice(MoveDirection direction)
    {
        if (currentState == GameState.WaitingForInput) moveChoice = direction;
    }

    public void ReceiveRotateChoice(RotateDirection direction)
    {
        if (currentState == GameState.WaitingForInput) rotateChoice = direction;
    }
}