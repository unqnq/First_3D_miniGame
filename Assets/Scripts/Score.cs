using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int playerScore = 0;
    public float scoreUpdateInterval = 1f;
    public TMP_Text scoreText;
    public int baseIncrease = 1;

    private TMP_Text finalScoreText;
    private float timer = 0f;

    void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        finalScoreText = GameObject.Find("FinalScoreText").GetComponent<TMP_Text>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= scoreUpdateInterval)
        {
            AddScore(baseIncrease);
            timer = 0f;
        }
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = playerScore.ToString();
    }

    public void AddScore(int increase)
    {
        playerScore += increase;
    }

    public void UpdateFinalScore()
    {
        finalScoreText.text = playerScore.ToString();
    }
}
