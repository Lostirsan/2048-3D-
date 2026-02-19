using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI countdownText;

    private Coroutine loseRoutine;
    private Cube currentCube;

    private bool isGameOver;

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void Awake()
    {
        Instance = this;

        if (losePanel != null)
            losePanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void CubeEnteredZone(Cube cube)
    {
        if (isGameOver) return;

        if (loseRoutine != null)
            StopCoroutine(loseRoutine);

        currentCube = cube;
        loseRoutine = StartCoroutine(LoseCountdown());
    }

    public void CubeExitedZone(Cube cube)
    {
        if (cube != currentCube) return;

        if (loseRoutine != null)
            StopCoroutine(loseRoutine);

        loseRoutine = null;
        currentCube = null;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    private IEnumerator LoseCountdown()
    {
        yield return new WaitForSeconds(1f);

        if (currentCube == null)
            yield break;

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);

            if (currentCube == null)
            {
                countdownText.gameObject.SetActive(false);
                yield break;
            }
        }

        countdownText.gameObject.SetActive(false);

        TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (losePanel != null)
            losePanel.SetActive(true);

        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
