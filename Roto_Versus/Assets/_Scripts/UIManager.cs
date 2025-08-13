// UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject moveCardsPanel;
    public GameObject rotateCardsPanel;

    // GameManager와 통신하기 위한 연결고리입니다.
    public GameManager gameManager;


    void Start()
    {
        // GameManager 연결고리가 없으면 에러 메시지를 표시합니다.
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in the UIManager!");
        }
        HideAllCards();
    }

    public void ShowMoveCards()
    {
        moveCardsPanel.SetActive(true);
        rotateCardsPanel.SetActive(false);
    }

    public void ShowRotateCards()
    {
        moveCardsPanel.SetActive(false);
        rotateCardsPanel.SetActive(true);
    }

    // --- 새로 추가 ---
    // 모든 카드를 숨기는 함수
    public void HideAllCards()
    {
        moveCardsPanel.SetActive(false);
        rotateCardsPanel.SetActive(false);
    }

    // --- 버튼들이 호출할 함수들 ---
    // 각 버튼은 자신이 어떤 행동인지 알려주며 GameManager의 함수를 호출합니다.
    public void OnMoveCardSelected(int direction)
    {
        // int로 받은 값을 MoveDirection 타입으로 변환하여 전달합니다.
        gameManager.ReceiveMoveChoice((MoveDirection)direction);
    }

    public void OnRotateCardSelected(int direction)
    {
        // int로 받은 값을 RotateDirection 타입으로 변환하여 전달합니다.
        gameManager.ReceiveRotateChoice((RotateDirection)direction);
    }
}