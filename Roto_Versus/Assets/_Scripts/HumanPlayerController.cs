// HumanPlayerController.cs ������ ��ü ����
using UnityEngine;

// PlayerController ���赵�� ��ü������ ������ '���' ��Ʈ�ѷ��Դϴ�.
public class HumanPlayerController : PlayerController
{
    // GameManager�� "�̵� �����ϼ���!" ��� ȣ���� �Լ�
    public override void ChooseMoveAction()
    {
        // �� HumanPlayerController�� �پ��ִ� ������Ʈ�� �̸��� ����մϴ�. (��: "P1_Controller")
        Debug.Log(gameObject.name + ": � �������� �����ϱ�? (�Է� �����...)");
    }

    // GameManager�� "ȸ�� �����ϼ���!" ��� ȣ���� �Լ�
    public override void ChooseRotateAction()
    {
        Debug.Log(gameObject.name + ": ���� ��� ȸ����ų��? (�Է� �����...)");
    }
}