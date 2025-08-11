// 유니티의 기능을 사용하기 위해 꼭 필요한 선언입니다.
using UnityEngine;

// BoardManager 클래스(설계도)를 정의합니다. MonoBehaviour를 상속받아 유니티 오브젝트의 컴포넌트로 작동할 수 있습니다.
public class BoardManager : MonoBehaviour
{
    // === 인스펙터 창에서 설정할 변수들 ===
    // 이 변수들은 public으로 선언되어 유니티 에디터의 Inspector 창에 노출되므로,
    // 코드를 바꾸지 않고도 값을 변경하거나 프리팹을 연결할 수 있습니다.

    [Header("Board Settings")] // 인스펙터 창에서 구분을 위한 헤더
    public int boardSize = 5;      // 보드의 크기 (5x5)
    public GameObject tilePrefab;      // 바닥 타일로 사용할 프리팹

    [Header("Unit Prefabs")] // 유닛 구분을 위한 헤더
    public GameObject player1Prefab;   // 플레이어 1로 사용할 프리팹
    public GameObject player2Prefab;   // 플레이어 2로 사용할 프리팹
    public GameObject goalPrefab;      // 목표 지점으로 사용할 프리팹


    // Start 함수는 게임이 시작될 때 단 한 번 자동으로 호출됩니다.
    // 게임의 무대를 설정하기에 가장 좋은 타이밍입니다.
    void Start()
    {
        // 1. 보드 타일들을 생성합니다.
        GenerateBoard();
        // 2. 플레이어와 목표 지점을 생성하고 배치합니다.
        SpawnUnits();
    }

    // 보드 타일을 생성하는 함수
    void GenerateBoard()
    {
        // for 반복문을 2번 중첩하여 격자(Grid)를 만듭니다.
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                // 월드 중앙(0,0,0)을 기준으로 타일을 배치하기 위해 위치를 보정합니다.
                // 예: boardSize가 5일때, x는 0~4. (x - 2)를 하면 -2, -1, 0, 1, 2 위치에 놓이게 됩니다.
                Vector3 tilePosition = new Vector3(x - (boardSize - 1) / 2f, 0, z - (boardSize - 1) / 2f);

                // Instantiate: 프리팹을 복제하여 씬에 실제로 생성하는 명령어입니다.
                // tilePrefab을 tilePosition 위치에, 기본 회전값(Quaternion.identity)으로 생성합니다.
                // 마지막의 this.transform은 생성된 타일들을 이 스크립트가 붙어있는 GameManager의 자식으로 만듭니다. (정리정돈 목적)
                Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform);
            }
        }
    }

    // 플레이어와 목표 지점을 생성하고 배치하는 함수
    void SpawnUnits()
    {
        // 보드의 가장자리 좌표를 계산합니다. (예: 5x5 보드에서는 2)
        float edgePos = (boardSize - 1) / 2f;

        // --- 여기를 수정합니다 ---
        // 플레이어 1은 왼쪽 아래에 배치 (Y 값을 0.51f로 변경)
        Vector3 player1Pos = new Vector3(-edgePos, 0.51f, -edgePos);
        Instantiate(player1Prefab, player1Pos, Quaternion.identity);

        // --- 여기도 수정합니다 ---
        // 플레이어 2는 오른쪽 위에 배치 (Y 값을 0.51f로 변경)
        Vector3 player2Pos = new Vector3(edgePos, 0.51f, edgePos);
        Instantiate(player2Prefab, player2Pos, Quaternion.identity);

        // 목표 지점은 정중앙에 배치 (Y=0.5f로 이미 잘 설정되어 있습니다)
        Vector3 goalPos = new Vector3(0, 0.5f, 0);
        Instantiate(goalPrefab, goalPos, Quaternion.identity);
    }
}