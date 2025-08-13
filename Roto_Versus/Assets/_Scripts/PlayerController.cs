// PlayerController.cs 파일의 올바른 내용
using UnityEngine;

// 모든 플레이어 컨트롤러(사람, AI)가 상속받아야 할 설계도(추상 클래스)입니다.
public abstract class PlayerController : MonoBehaviour
{
    // 이동 방향 결정을 요청하는 함수 (자식 클래스에서 반드시 구현해야 함)
    public abstract void ChooseMoveAction();

    // 맵 회전 방향 결정을 요청하는 함수 (자식 클래스에서 반드시 구현해야 함)
    public abstract void ChooseRotateAction();
}