// PlayerController.cs ������ �ùٸ� ����
using UnityEngine;

// ��� �÷��̾� ��Ʈ�ѷ�(���, AI)�� ��ӹ޾ƾ� �� ���赵(�߻� Ŭ����)�Դϴ�.
public abstract class PlayerController : MonoBehaviour
{
    // �̵� ���� ������ ��û�ϴ� �Լ� (�ڽ� Ŭ�������� �ݵ�� �����ؾ� ��)
    public abstract void ChooseMoveAction();

    // �� ȸ�� ���� ������ ��û�ϴ� �Լ� (�ڽ� Ŭ�������� �ݵ�� �����ؾ� ��)
    public abstract void ChooseRotateAction();
}