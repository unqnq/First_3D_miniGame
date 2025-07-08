using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int playerScore = 0;
    public TMP_Text scoreText;
    public Transform player;
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        playerScore = ((int)player.position.z)/2;
        scoreText.text = playerScore.ToString();
    }
}
