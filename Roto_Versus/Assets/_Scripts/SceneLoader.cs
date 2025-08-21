using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 꼭 필요합니다.

public class SceneLoader : MonoBehaviour
{
    // string 타입의 씬 이름을 받아서 해당 씬을 로드하는 함수입니다.
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}