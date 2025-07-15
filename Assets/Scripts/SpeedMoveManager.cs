using UnityEngine;

public class SpeedMoveManager : MonoBehaviour
{
    public float startSpeed = 45f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 100f;

    public float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
