using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private GameManager gameManager;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = FindFirstObjectByType<GameManager>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "DeadZone")
        {
            playerMovement.enabled = false;
            gameManager.GameOver();
        }
    }

}
