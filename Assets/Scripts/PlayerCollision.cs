using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private GameManager gameManager;
    private SpeedMoveManager speedManager;
    private GameObject coinEffect;
    private ObjectPool pool;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = FindFirstObjectByType<GameManager>();
        coinEffect = Resources.Load<GameObject>("CoinEffect");
        pool = FindFirstObjectByType<ObjectPool>();
        speedManager = FindAnyObjectByType<SpeedMoveManager>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "DeadZone")
        {
            playerMovement.enabled = false;
            speedManager.currentSpeed = 0;
            gameManager.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Score score = gameManager.GetComponent<Score>();
            score.AddScore(10);
            score.UpdateScore();

            StartCoroutine(HandleCoinPickup(other.gameObject));
        }
    }

    IEnumerator HandleCoinPickup(GameObject coin)
    {
        coin.GetComponent<MeshRenderer>().enabled = false;

        GameObject coinFx = Instantiate(coinEffect, coin.transform);
        yield return new WaitForSeconds(2f);

        if (coinFx != null)
        {
            Destroy(coinFx, 3);
        }
        pool.ReturnToPool(coin.gameObject);

        coin.GetComponent<MeshRenderer>().enabled = true;
    }

}
