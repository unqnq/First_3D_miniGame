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
        UpdateScore();
    }

    void UpdateScore()
    {
        timer += Time.deltaTime;
        if (timer >= scoreUpdateInterval)
        {
            playerScore += 1;
            timer = 0f;
            scoreText.text = playerScore.ToString();
        }
    }
}
