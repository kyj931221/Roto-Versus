// GameManager.cs (���� ���׷��̵� ����)
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player1Controller;
    public PlayerController player2Controller;
    public UIManager uiManager;
    public BoardManager boardManager; // BoardManager�� ����ϱ� ���� �����

    public enum GameState { TurnStart, WaitingForInput, TurnResolution, GameOver }
    private GameState currentState;

    private int turnNumber = 1;
    private bool isPlayer1Mover = true;

    // �÷��̾��� ���ð� ���� ��ġ�� ������ ����
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

    // �÷��̾��� �ʱ� ��ġ�� �����ϴ� �Լ�
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

        // --- 1. �̵� ���� ---
        currentState = GameState.WaitingForInput;
        Debug.Log(mover.name + "'s turn to choose a MOVE.");
        uiManager.ShowMoveCards();
        while (moveChoice == null) { yield return null; }

        // --- 2. ȸ�� ���� ---
        currentState = GameState.WaitingForInput;
        Debug.Log(rotator.name + "'s turn to choose a ROTATION.");
        uiManager.ShowRotateCards();
        while (rotateChoice == null) { yield return null; }

        uiManager.HideAllCards();

        // --- 3. �� ����! ---
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

        // --- ��� üũ ���� �߰� ---
        // ���� �����ڸ� ��ǥ ��� (��: 5x5 ������ ��� maxCoord = 2)
        int maxCoord = (boardManager.boardSize - 1) / 2;

        // Clamp �Լ��� �̿��� �÷��̾��� x, y ��ǥ�� (-maxCoord ~ maxCoord) ������ ����� �ʵ��� ��
        player1GridPos.x = Mathf.Clamp(player1GridPos.x, -maxCoord, maxCoord);
        player1GridPos.y = Mathf.Clamp(player1GridPos.y, -maxCoord, maxCoord);

        player2GridPos.x = Mathf.Clamp(player2GridPos.x, -maxCoord, maxCoord);
        player2GridPos.y = Mathf.Clamp(player2GridPos.y, -maxCoord, maxCoord);
        // --- ��� üũ ���� �� ---

        Vector3 p1TargetWorldPos = new Vector3(player1GridPos.x, 0.51f, player1GridPos.y);
        Vector3 p2TargetWorldPos = new Vector3(player2GridPos.x, 0.51f, player2GridPos.y);

        yield return StartCoroutine(boardManager.player1Instance.GetComponent<PlayerPawn>().MoveRoutine(p1TargetWorldPos));
        yield return StartCoroutine(boardManager.player2Instance.GetComponent<PlayerPawn>().MoveRoutine(p2TargetWorldPos));

        // TODO: �¸� ���� üũ ���� �߰� �ʿ�

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