// ����Ƽ�� ����� ����ϱ� ���� �� �ʿ��� �����Դϴ�.
using UnityEngine;
// �ڷ�ƾ(IEnumerator)�� ����ϱ� ���� �߰��ؾ� �ϴ� �����Դϴ�.
using System.Collections;

// BoardManager Ŭ����(���赵)�� �����մϴ�. MonoBehaviour�� ��ӹ޾� ����Ƽ ������Ʈ�� ������Ʈ�� �۵��� �� �ֽ��ϴ�.
public class BoardManager : MonoBehaviour
{
    // === �ν����� â���� ������ ������ ===
    [Header("Board Settings")] // �ν����� â���� ������ ���� ���
    public int boardSize = 5;       // ������ ũ�� (5x5)
    public GameObject tilePrefab;      // �ٴ� Ÿ�Ϸ� ����� ������

    [Header("Unit Prefabs")] // ���� ������ ���� ���
    public GameObject player1Prefab;   // �÷��̾� 1�� ����� ������
    public GameObject player2Prefab;   // �÷��̾� 2�� ����� ������
    public GameObject goalPrefab;      // ��ǥ �������� ����� ������

    // ������ �÷��̾� ������Ʈ�� �����Ͽ� �ٸ� ��ũ��Ʈ�� ������ �� �ֵ��� �մϴ�.
    [HideInInspector] public GameObject player1Instance;
    [HideInInspector] public GameObject player2Instance;


    // Start �Լ��� ������ ���۵� �� �� �� �� �ڵ����� ȣ��˴ϴ�.
    void Start()
    {
        // 1. ���� Ÿ�ϵ��� �����մϴ�.
        GenerateBoard();
        // 2. �÷��̾�� ��ǥ ������ �����ϰ� ��ġ�մϴ�.
        SpawnUnits();
    }

    // ���� Ÿ���� �����ϴ� �Լ�
    void GenerateBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                Vector3 tilePosition = new Vector3(x - (boardSize - 1) / 2f, 0, z - (boardSize - 1) / 2f);
                Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform);
            }
        }
    }

    // �÷��̾�� ��ǥ ������ �����ϰ� ��ġ�ϴ� �Լ�
    void SpawnUnits()
    {
        float edgePos = (boardSize - 1) / 2f;

        Vector3 player1Pos = new Vector3(-edgePos, 0.51f, -edgePos);
        player1Instance = Instantiate(player1Prefab, player1Pos, Quaternion.identity);

        Vector3 player2Pos = new Vector3(edgePos, 0.51f, edgePos);
        player2Instance = Instantiate(player2Prefab, player2Pos, Quaternion.identity);

        Vector3 goalPos = new Vector3(0, 0.5f, 0);
        Instantiate(goalPrefab, goalPos, Quaternion.identity);
    }

    // ���带 �ε巴�� ȸ����Ű�� �ڷ�ƾ �Լ�
    public IEnumerator RotateBoardRoutine(RotateDirection direction)
    {
        float duration = 0.5f; // ȸ���� �ɸ��� �ð� (��)
        float elapsed = 0f;

        Quaternion startRotation = transform.rotation;
        float angle = direction == RotateDirection.Right ? 90f : -90f;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, angle, 0);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}