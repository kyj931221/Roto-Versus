// UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject moveCardsPanel;
    public GameObject rotateCardsPanel;

    // GameManager�� ����ϱ� ���� ������Դϴ�.
    public GameManager gameManager;


    void Start()
    {
        // GameManager ������� ������ ���� �޽����� ǥ���մϴ�.
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

    // --- ���� �߰� ---
    // ��� ī�带 ����� �Լ�
    public void HideAllCards()
    {
        moveCardsPanel.SetActive(false);
        rotateCardsPanel.SetActive(false);
    }

    // --- ��ư���� ȣ���� �Լ��� ---
    // �� ��ư�� �ڽ��� � �ൿ���� �˷��ָ� GameManager�� �Լ��� ȣ���մϴ�.
    public void OnMoveCardSelected(int direction)
    {
        // int�� ���� ���� MoveDirection Ÿ������ ��ȯ�Ͽ� �����մϴ�.
        gameManager.ReceiveMoveChoice((MoveDirection)direction);
    }

    public void OnRotateCardSelected(int direction)
    {
        // int�� ���� ���� RotateDirection Ÿ������ ��ȯ�Ͽ� �����մϴ�.
        gameManager.ReceiveRotateChoice((RotateDirection)direction);
    }
}