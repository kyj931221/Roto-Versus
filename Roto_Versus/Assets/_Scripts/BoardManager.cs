// ����Ƽ�� ����� ����ϱ� ���� �� �ʿ��� �����Դϴ�.
using UnityEngine;

// BoardManager Ŭ����(���赵)�� �����մϴ�. MonoBehaviour�� ��ӹ޾� ����Ƽ ������Ʈ�� ������Ʈ�� �۵��� �� �ֽ��ϴ�.
public class BoardManager : MonoBehaviour
{
    // === �ν����� â���� ������ ������ ===
    // �� �������� public���� ����Ǿ� ����Ƽ �������� Inspector â�� ����ǹǷ�,
    // �ڵ带 �ٲ��� �ʰ� ���� �����ϰų� �������� ������ �� �ֽ��ϴ�.

    [Header("Board Settings")] // �ν����� â���� ������ ���� ���
    public int boardSize = 5;      // ������ ũ�� (5x5)
    public GameObject tilePrefab;      // �ٴ� Ÿ�Ϸ� ����� ������

    [Header("Unit Prefabs")] // ���� ������ ���� ���
    public GameObject player1Prefab;   // �÷��̾� 1�� ����� ������
    public GameObject player2Prefab;   // �÷��̾� 2�� ����� ������
    public GameObject goalPrefab;      // ��ǥ �������� ����� ������


    // Start �Լ��� ������ ���۵� �� �� �� �� �ڵ����� ȣ��˴ϴ�.
    // ������ ���븦 �����ϱ⿡ ���� ���� Ÿ�̹��Դϴ�.
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
        // for �ݺ����� 2�� ��ø�Ͽ� ����(Grid)�� ����ϴ�.
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                // ���� �߾�(0,0,0)�� �������� Ÿ���� ��ġ�ϱ� ���� ��ġ�� �����մϴ�.
                // ��: boardSize�� 5�϶�, x�� 0~4. (x - 2)�� �ϸ� -2, -1, 0, 1, 2 ��ġ�� ���̰� �˴ϴ�.
                Vector3 tilePosition = new Vector3(x - (boardSize - 1) / 2f, 0, z - (boardSize - 1) / 2f);

                // Instantiate: �������� �����Ͽ� ���� ������ �����ϴ� ��ɾ��Դϴ�.
                // tilePrefab�� tilePosition ��ġ��, �⺻ ȸ����(Quaternion.identity)���� �����մϴ�.
                // �������� this.transform�� ������ Ÿ�ϵ��� �� ��ũ��Ʈ�� �پ��ִ� GameManager�� �ڽ����� ����ϴ�. (�������� ����)
                Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform);
            }
        }
    }

    // �÷��̾�� ��ǥ ������ �����ϰ� ��ġ�ϴ� �Լ�
    void SpawnUnits()
    {
        // ������ �����ڸ� ��ǥ�� ����մϴ�. (��: 5x5 ���忡���� 2)
        float edgePos = (boardSize - 1) / 2f;

        // --- ���⸦ �����մϴ� ---
        // �÷��̾� 1�� ���� �Ʒ��� ��ġ (Y ���� 0.51f�� ����)
        Vector3 player1Pos = new Vector3(-edgePos, 0.51f, -edgePos);
        Instantiate(player1Prefab, player1Pos, Quaternion.identity);

        // --- ���⵵ �����մϴ� ---
        // �÷��̾� 2�� ������ ���� ��ġ (Y ���� 0.51f�� ����)
        Vector3 player2Pos = new Vector3(edgePos, 0.51f, edgePos);
        Instantiate(player2Prefab, player2Pos, Quaternion.identity);

        // ��ǥ ������ ���߾ӿ� ��ġ (Y=0.5f�� �̹� �� �����Ǿ� �ֽ��ϴ�)
        Vector3 goalPos = new Vector3(0, 0.5f, 0);
        Instantiate(goalPrefab, goalPos, Quaternion.identity);
    }
}