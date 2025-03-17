using UnityEngine;

public class DirectionalBrick : Brick
{
    [Header("Directional Settings")]
    [SerializeField] private Vector2 bounceDirection = Vector2.up;
    [SerializeField] private float bounceStrength = 1.2f;
    [SerializeField] private Color directionIndicatorColor = Color.cyan;
    
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        // Set a tint color to indicate this is a directional brick
        if (spriteRenderer != null)
        {
            spriteRenderer.color = directionIndicatorColor;
        }
        
        // Normalize the bounce direction
        bounceDirection = bounceDirection.normalized;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Change the ball's direction
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                // Get the current speed
                float currentSpeed = ballRb.velocity.magnitude;
                
                // Apply the directional force
                ballRb.velocity = bounceDirection * currentSpeed * bounceStrength;
            }
            
            // Also handle the normal brick behavior (take damage)
            HandleBallCollision();
        }
    }
    
    // Optional: Override the regular ball collision to ensure our direction change happens
    protected override void HandleBallCollision()
    {
        base.HandleBallCollision();
    }
    
    // For editor visualization of the direction
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = directionIndicatorColor;
        Vector3 direction = (Vector3)bounceDirection.normalized;
        Gizmos.DrawLine(transform.position, transform.position + direction);
        
        // Draw an arrow head
        Vector3 arrowStart = transform.position + direction;
        Vector3 right = Quaternion.Euler(0, 0, -30) * -direction * 0.2f;
        Vector3 left = Quaternion.Euler(0, 0, 30) * -direction * 0.2f;
        Gizmos.DrawLine(arrowStart, arrowStart + right);
        Gizmos.DrawLine(arrowStart, arrowStart + left);
    }
} 