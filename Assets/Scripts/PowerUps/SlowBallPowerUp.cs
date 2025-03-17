using UnityEngine;
using System.Collections;

public class SlowBallPowerUp : PowerUp
{
    [Header("Slow Ball Settings")]
    [SerializeField] private float speedMultiplier = 0.5f;
    
    protected override void ApplyEffect()
    {
        // Find all balls in the scene
        BallController[] balls = FindObjectsOfType<BallController>();
        
        if (balls.Length > 0)
        {
            StartCoroutine(ApplyTimedEffect(
                () => SlowAllBalls(balls),
                () => RestoreBallSpeed(balls)
            ));
        }
    }
    
    private void SlowAllBalls(BallController[] balls)
    {
        foreach (BallController ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.magnitude > 0)
            {
                // Store original velocity magnitude in a component that will persist
                BallSpeedModifier modifier = ball.gameObject.GetComponent<BallSpeedModifier>();
                if (modifier == null)
                {
                    modifier = ball.gameObject.AddComponent<BallSpeedModifier>();
                }
                
                modifier.StoreOriginalSpeed(rb.velocity.magnitude);
                
                // Apply slow effect
                rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude * speedMultiplier);
            }
        }
    }
    
    private void RestoreBallSpeed(BallController[] balls)
    {
        foreach (BallController ball in balls)
        {
            // Ball might have been destroyed
            if (ball == null) continue;
            
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            BallSpeedModifier modifier = ball.GetComponent<BallSpeedModifier>();
            
            if (rb != null && modifier != null)
            {
                // Restore original speed
                float originalSpeed = modifier.GetOriginalSpeed();
                if (originalSpeed > 0)
                {
                    rb.velocity = rb.velocity.normalized * originalSpeed;
                }
                
                // Clean up the component
                Destroy(modifier);
            }
        }
    }
}

// Helper component to store the original ball speed
public class BallSpeedModifier : MonoBehaviour
{
    private float originalSpeed;
    
    public void StoreOriginalSpeed(float speed)
    {
        originalSpeed = speed;
    }
    
    public float GetOriginalSpeed()
    {
        return originalSpeed;
    }
} 