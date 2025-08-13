// PlayerPawn.cs 파일에 추가
using System.Collections;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    // 말을 부드럽게 움직이는 코루틴 함수
    public IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        float duration = 0.3f; // 이동에 걸리는 시간 (초)
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}