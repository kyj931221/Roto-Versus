using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Component Links")]
    public GameManager gameManager;

    [Header("Main UI Containers")]
    public RectTransform mapContainer;
    public GameObject p1Area;
    public GameObject p2Area;

    [Header("Player 1 UI Elements")]
    public GameObject p1RPSCards;
    public GameObject p1MoveCards;
    public GameObject p1RotateCards;
    public TextMeshProUGUI p1RoleText;
    public TextMeshProUGUI p1StatusText;

    [Header("Player 2 UI Elements")]
    public GameObject p2RPSCards;
    public GameObject p2MoveCards;
    public GameObject p2RotateCards;
    public TextMeshProUGUI p2RoleText;
    public TextMeshProUGUI p2StatusText;

    [Header("System Panels")]
    public GameObject winnerChoicePanel;
    public GameObject gameOverPanel;

    [Header("System Texts")]
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI resultText;

    private Vector2 mapCenterPos;
    private Vector2 mapTargetForP1;
    private Vector2 mapTargetForP2;

    void Start()
    {
        mapCenterPos = mapContainer.anchoredPosition;
        float moveDistance = (GetComponent<RectTransform>().rect.height) / 4f;
        mapTargetForP2 = new Vector2(mapCenterPos.x, mapCenterPos.y - moveDistance);
        mapTargetForP1 = new Vector2(mapCenterPos.x, mapCenterPos.y + moveDistance);

        p1Area.SetActive(false);
        p2Area.SetActive(false);
        infoText.gameObject.SetActive(false);
        winnerChoicePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowRPSUI()
    {
        infoText.gameObject.SetActive(true);
        infoText.text = "가위바위보!";

        p1Area.SetActive(true);
        p1RPSCards.SetActive(true);
        p1MoveCards.SetActive(false);
        p1RotateCards.SetActive(false);
        p1StatusText.text = "선택하세요...";
        p1RoleText.text = "";

        p2Area.SetActive(true);
        p2RPSCards.SetActive(true);
        p2MoveCards.SetActive(false);
        p2RotateCards.SetActive(false);
        p2StatusText.text = "선택하세요...";
        p2RoleText.text = "";

        winnerChoicePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void UpdateRPSStatus(int player, string status)
    {
        if (player == 1) p1StatusText.text = status;
        else p2StatusText.text = status;
    }

    public void ShowRPSResult(string resultMessage)
    {
        infoText.text = resultMessage;
    }

    public void ShowWinnerChoiceUI(int winnerNum)
    {
        p1RPSCards.SetActive(false);
        p2RPSCards.SetActive(false);
        p1StatusText.text = "";
        p2StatusText.text = "";
        infoText.gameObject.SetActive(false);

        GameObject winnerArea = (winnerNum == 1) ? p1Area : p2Area;
        GameObject loserArea = (winnerNum == 1) ? p2Area : p1Area;

        winnerArea.SetActive(true);
        loserArea.SetActive(false);

        winnerChoicePanel.transform.SetParent(winnerArea.transform, false);
        winnerChoicePanel.SetActive(true);
        winnerText.text = (winnerNum == 1 ? "Player 1" : "Player 2") + " WINS!\nChoose Order";
    }

    public void HideAllSetupUI()
    {
        winnerChoicePanel.SetActive(false);
        p1Area.SetActive(false);
        p2Area.SetActive(false);
    }

    public void ShowPlayerUI(int player, bool isMover)
    {
        GameObject area = (player == 1) ? p1Area : p2Area;
        GameObject moveCards = (player == 1) ? p1MoveCards : p2MoveCards;
        GameObject rotateCards = (player == 1) ? p1RotateCards : p2RotateCards;
        TextMeshProUGUI roleText = (player == 1) ? p1RoleText : p2RoleText;

        area.SetActive(true);
        if (isMover)
        {
            moveCards.SetActive(true);
            rotateCards.SetActive(false);
            roleText.text = "YOUR TURN: MOVE";
        }
        else
        {
            moveCards.SetActive(false);
            rotateCards.SetActive(true);
            roleText.text = "YOUR TURN: ROTATE";
        }
    }

    public void HidePlayerUI(int player)
    {
        if (player == 1) p1Area.SetActive(false);
        else p2Area.SetActive(false);
    }

    public IEnumerator AnimateMapTransition(int targetState)
    {
        float duration = 0.4f;
        float elapsed = 0f;
        Vector2 startPos = mapContainer.anchoredPosition;
        Vector2 endPos;

        if (targetState == 0) endPos = mapCenterPos;
        else if (targetState == 1) endPos = mapTargetForP1;
        else endPos = mapTargetForP2;

        while (elapsed < duration)
        {
            mapContainer.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mapContainer.anchoredPosition = endPos;
    }

    public void ShowGameOverScreen(string message)
    {
        p1Area.SetActive(false);
        p2Area.SetActive(false);
        gameOverPanel.SetActive(true);
        resultText.text = message;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnP1_SelectRock() { gameManager.ReceiveRPSChoice(1, 0); }
    public void OnP1_SelectPaper() { gameManager.ReceiveRPSChoice(1, 1); }
    public void OnP1_SelectScissors() { gameManager.ReceiveRPSChoice(1, 2); }
    public void OnP2_SelectRock() { gameManager.ReceiveRPSChoice(2, 0); }
    public void OnP2_SelectPaper() { gameManager.ReceiveRPSChoice(2, 1); }
    public void OnP2_SelectScissors() { gameManager.ReceiveRPSChoice(2, 2); }
    public void OnFirstGoSelected() { gameManager.ReceiveWinnerChoice(true); }
    public void OnSecondGoSelected() { gameManager.ReceiveWinnerChoice(false); }
}