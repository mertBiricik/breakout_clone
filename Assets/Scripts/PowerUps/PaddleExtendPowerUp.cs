using UnityEngine;
using System.Collections;

public class PaddleExtendPowerUp : PowerUp
{
    [Header("Paddle Extend Settings")]
    [SerializeField] private float scaleMultiplier = 1.5f;
    
    protected override void ApplyEffect()
    {
        // Find the paddle
        GameObject paddle = GameObject.FindGameObjectWithTag("Paddle");
        
        if (paddle != null)
        {
            // Start the timed effect
            StartCoroutine(ApplyTimedEffect(
                () => ExtendPaddle(paddle),
                () => RestorePaddle(paddle)
            ));
        }
    }
    
    private void ExtendPaddle(GameObject paddle)
    {
        // Store the original scale for restoration later
        paddle.GetComponent<PaddleController>()?.StoreOriginalScale();
        
        // Increase the paddle's x scale
        Vector3 newScale = paddle.transform.localScale;
        newScale.x *= scaleMultiplier;
        paddle.transform.localScale = newScale;
    }
    
    private void RestorePaddle(GameObject paddle)
    {
        // Restore the paddle's original scale
        paddle.GetComponent<PaddleController>()?.RestoreOriginalScale();
    }
} 