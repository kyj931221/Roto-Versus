// 유니티의 기능을 사용하기 위해 꼭 필요한 선언입니다.
using UnityEngine;
// 코루틴(IEnumerator)을 사용하기 위해 추가해야 하는 선언입니다.
using System.Collections;

// BoardManager 클래스(설계도)를 정의합니다. MonoBehaviour를 상속받아 유니티 오브젝트의 컴포넌트로 작동할 수 있습니다.
public class BoardManager : MonoBehaviour
{
    // === 인스펙터 창에서 설정할 변수들 ===
    [Header("Board Settings")] // 인스펙터 창에서 구분을 위한 헤더
    public int boardSize = 5;       // 보드의 크기 (5x5)
    public GameObject tilePrefab;      // 바닥 타일로 사용할 프리팹

    [Header("Unit Prefabs")] // 유닛 구분을 위한 헤더
    public GameObject player1Prefab;   // 플레이어 1로 사용할 프리팹
    public GameObject player2Prefab;   // 플레이어 2로 사용할 프리팹
    public GameObject goalPrefab;      // 목표 지점으로 사용할 프리팹

    // 생성된 플레이어 오브젝트를 저장하여 다른 스크립트가 접근할 수 있도록 합니다.
    [HideInInspector] public GameObject player1Instance;
    [HideInInspector] public GameObject player2Instance;


    // Start 함수는 게임이 시작될 때 단 한 번 자동으로 호출됩니다.
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
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                Vector3 tilePosition = new Vector3(x - (boardSize - 1) / 2f, 0, z - (boardSize - 1) / 2f);
                Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform);
            }
        }
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
        float duration = 0.5f; // 회전에 걸리는 시간 (초)
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