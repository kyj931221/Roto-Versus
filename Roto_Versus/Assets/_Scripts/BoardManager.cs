// 유니티의 기능을 사용하기 위해 꼭 필요한 선언입니다.
using UnityEngine;
// 코루틴(IEnumerator)을 사용하기 위해 추가해야 하는 선언입니다.
using System.Collections;

// BoardManager 클래스(설계도)를 정의합니다. MonoBehaviour를 상속받아 유니티 오브젝트의 컴포넌트로 작동할 수 있습니다.
public class BoardManager : MonoBehaviour
{
    // === 인스펙터 창에서 설정할 변수들 ===
    [Header("Board Settings")]
    public int boardSize = 5;
    public GameObject tilePrefab;

    [Header("Unit Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject goalPrefab;
    public GameObject directionMarkerPrefab; // 방향 표시기 프리팹을 연결할 변수

    // 생성된 플레이어 오브젝트를 저장하여 다른 스크립트가 접근할 수 있도록 합니다.
    [HideInInspector] public GameObject player1Instance;
    [HideInInspector] public GameObject player2Instance;


    // GameManager가 호출할 보드 생성 함수
    public void CreateBoard()
    {
        // 1. 타일들을 생성합니다.
        GenerateBoard();

        // 2. 방향 표시기를 생성합니다.
        SpawnDirectionMarker();

        // 3. 플레이어와 목표 지점을 생성하고 배치합니다.
        SpawnUnits();
    }

    // 보드 타일을 생성하는 함수
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

    // 방향 표시기를 생성하는 함수
    void SpawnDirectionMarker()
    {
        if (directionMarkerPrefab == null) return; // 프리팹이 연결 안됐으면 실행 안함

        // 맵의 '정면' 가장자리 중앙 좌표를 계산합니다. (예: 5x5 맵에서는 (0, 0, 2))
        int maxCoord = (boardSize - 1) / 2;
        Vector3 markerPos = new Vector3(0, 0.55f, maxCoord); // 타일보다 살짝 위에 배치

        // 표시기를 생성하고, 보드와 함께 회전하도록 자식으로 만듭니다.
        Instantiate(directionMarkerPrefab, markerPos, Quaternion.identity, this.transform);
    }

    // 플레이어와 목표 지점을 생성하고 배치하는 함수
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

    // 보드를 부드럽게 회전시키는 코루틴 함수
    public IEnumerator RotateBoardRoutine(RotateDirection direction)
    {
        float duration = 0.5f;
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