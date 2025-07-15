using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Score score = GetComponent<Score>();
        score.baseIncrease = 0;
        score.UpdateFinalScore();
        StartCoroutine(WaitAndPause(1f));
    }

    IEnumerator WaitAndPause(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Restarted");
        Time.timeScale = 1f;
    }
}
