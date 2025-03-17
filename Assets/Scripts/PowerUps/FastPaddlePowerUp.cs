using UnityEngine;
using System.Collections;

public class FastPaddlePowerUp : PowerUp
{
    [Header("Fast Paddle Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    
    protected override void ApplyEffect()
    {
        // Find the paddle controller
        PaddleController paddleController = FindObjectOfType<PaddleController>();
        
        if (paddleController != null)
        {
            StartCoroutine(ApplyTimedEffect(
                () => SpeedUpPaddle(paddleController),
                () => RestorePaddleSpeed(paddleController)
            ));
        }
    }
    
    private void SpeedUpPaddle(PaddleController paddleController)
    {
        // Store and increase the paddle speed
        PaddleSpeedModifier modifier = paddleController.gameObject.GetComponent<PaddleSpeedModifier>();
        if (modifier == null)
        {
            modifier = paddleController.gameObject.AddComponent<PaddleSpeedModifier>();
            modifier.StoreOriginalSpeed(paddleController.GetSpeed());
        }
        
        paddleController.SetSpeed(modifier.GetOriginalSpeed() * speedMultiplier);
    }
    
    private void RestorePaddleSpeed(PaddleController paddleController)
    {
        // Restore the original paddle speed
        PaddleSpeedModifier modifier = paddleController.GetComponent<PaddleSpeedModifier>();
        if (modifier != null)
        {
            paddleController.SetSpeed(modifier.GetOriginalSpeed());
            Destroy(modifier);
        }
    }
}

// Helper component to store the original paddle speed
public class PaddleSpeedModifier : MonoBehaviour
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