// PlayerPawn.cs ���Ͽ� �߰�
using System.Collections;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    // ���� �ε巴�� �����̴� �ڷ�ƾ �Լ�
    public IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        float duration = 0.3f; // �̵��� �ɸ��� �ð� (��)
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