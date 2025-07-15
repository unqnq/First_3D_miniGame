using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int playerScore = 0;
    public float scoreUpdateInterval = 1f;
    public TMP_Text scoreText;

    private float timer = 0f;
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();

    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= scoreUpdateInterval)
        {
            AddScore(1);
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
}
