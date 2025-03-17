using UnityEngine;
using System.Collections;

public class MultiBallPowerUp : PowerUp
{
    [Header("Multi-Ball Settings")]
    [SerializeField] private int ballCount = 3;
    [SerializeField] private float spreadAngle = 20f;
    [SerializeField] private GameObject ballPrefab;
    
    protected override void ApplyEffect()
    {
        // Find the existing ball to clone
        BallController originalBall = FindObjectOfType<BallController>();
        
        if (originalBall != null)
        {
            SpawnBalls(originalBall);
        }
    }
    
    private void SpawnBalls(BallController originalBall)
    {
        // Only spawn balls if the original ball is launched
        Rigidbody2D originalRb = originalBall.GetComponent<Rigidbody2D>();
        if (originalRb.velocity.magnitude == 0) return;
        
        Vector2 originalVelocity = originalRb.velocity;
        float originalSpeed = originalVelocity.magnitude;
        
        // Calculate the base angle
        float baseAngle = Mathf.Atan2(originalVelocity.y, originalVelocity.x) * Mathf.Rad2Deg;
        
        // Spawn additional balls
        for (int i = 0; i < ballCount; i++)
        {
            // Calculate a spread angle for this ball
            float angle = baseAngle + Random.Range(-spreadAngle, spreadAngle);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            
            // Create the new ball
            GameObject newBallObj;
            if (ballPrefab != null)
            {
                newBallObj = Instantiate(ballPrefab, originalBall.transform.position, Quaternion.identity);
            }
            else
            {
                newBallObj = Instantiate(originalBall.gameObject, originalBall.transform.position, Quaternion.identity);
            }
            
            // Set its velocity
            Rigidbody2D rb = newBallObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * originalSpeed;
            }
            
            // Make sure the new ball is launched
            BallController newBall = newBallObj.GetComponent<BallController>();
            if (newBall != null)
            {
                newBall.SetBallLaunched(true);
            }
        }
    }
} 