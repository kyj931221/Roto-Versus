using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �� �ʿ��մϴ�.

public class SceneLoader : MonoBehaviour
{
    // string Ÿ���� �� �̸��� �޾Ƽ� �ش� ���� �ε��ϴ� �Լ��Դϴ�.
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}