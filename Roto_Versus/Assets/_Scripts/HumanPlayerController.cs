// HumanPlayerController.cs 파일의 전체 내용
using UnityEngine;

// PlayerController 설계도를 구체적으로 구현한 '사람' 컨트롤러입니다.
public class HumanPlayerController : PlayerController
{
    // GameManager가 "이동 결정하세요!" 라고 호출할 함수
    public override void ChooseMoveAction()
    {
        // 이 HumanPlayerController가 붙어있는 오브젝트의 이름을 출력합니다. (예: "P1_Controller")
        Debug.Log(gameObject.name + ": 어떤 방향으로 움직일까? (입력 대기중...)");
    }

    // GameManager가 "회전 결정하세요!" 라고 호출할 함수
    public override void ChooseRotateAction()
    {
        Debug.Log(gameObject.name + ": 맵을 어떻게 회전시킬까? (입력 대기중...)");
    }
}