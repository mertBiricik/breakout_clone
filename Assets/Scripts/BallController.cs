using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float initialSpeed = 7f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrement = 0.1f;
    
    [Header("References")]
    [SerializeField] private Transform paddleTransform;
    
    private Rigidbody2D rb;
    private Vector2 ballDirection;
    private bool isBallLaunched = false;
    private Vector2 paddleToBallOffset;
    private GameManager gameManager;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        gameManager = GameManager.Instance;
        
        // If paddleTransform is not assigned (like for cloned balls from power-up)
        if (paddleTransform == null)
        {
            GameObject paddle = GameObject.FindGameObjectWithTag("Paddle");
            if (paddle != null)
            {
                paddleTransform = paddle.transform;
            }
        }
        
        if (paddleTransform != null)
        {
            paddleToBallOffset = transform.position - paddleTransform.position;
        }
        
        if (!isBallLaunched)
        {
            ResetBall();
        }
    }
    
    void Update()
    {
        // If the ball isn't launched yet, keep it on the paddle
        if (!isBallLaunched)
        {
            // Position the ball on top of the paddle
            if (paddleTransform != null)
            {
                transform.position = paddleTransform.position + (Vector3)paddleToBallOffset;
            }
            
            // Launch the ball when the player presses the space key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchBall();
            }
        }
        
        // Cap the ball velocity to max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        // Fix the issue where ball bounces horizontally between walls too many times
        PreventInfiniteHorizontalBouncing();
    }
    
    void LaunchBall()
    {
        isBallLaunched = true;
        rb.velocity = Vector2.up * initialSpeed;
    }
    
    public void ResetBall()
    {
        isBallLaunched = false;
        rb.velocity = Vector2.zero;
        
        if (paddleTransform != null)
        {
            transform.position = paddleTransform.position + (Vector3)paddleToBallOffset;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Add a small boost to the ball's speed with each collision
        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + speedIncrement);
        
        // Add some randomness to the bounce if hitting the paddle
        if (collision.gameObject.CompareTag("Paddle"))
        {
            AddBounceVariation(collision);
        }
    }
    
    void AddBounceVariation(Collision2D collision)
    {
        // Calculate the hit position relative to the paddle center
        float hitPos = transform.position.x - collision.transform.position.x;
        // Normalize the hit position based on paddle width
        float paddleHalfWidth = collision.collider.bounds.size.x / 2;
        float normalizedHitPos = hitPos / paddleHalfWidth;
        
        // Calculate the bounce angle based on where the ball hit the paddle
        float bounceAngle = normalizedHitPos * 60f; // Max 60 degrees left or right
        
        // Create the new velocity vector
        Vector2 newDirection = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        rb.velocity = newDirection * rb.velocity.magnitude;
    }
    
    void PreventInfiniteHorizontalBouncing()
    {
        // If the ball is moving almost perfectly horizontally, add a small vertical component
        if (Mathf.Abs(rb.velocity.y) < 0.5f && rb.velocity.magnitude > 0)
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity.y = rb.velocity.magnitude * 0.2f * Mathf.Sign(rb.velocity.y);
            if (newVelocity.y == 0) newVelocity.y = rb.velocity.magnitude * 0.2f;
            rb.velocity = newVelocity;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
        {
            gameManager.LoseBall();
            ResetBall();
        }
    }
    
    // Used by power-ups to control the ball's state
    public void SetBallLaunched(bool launched)
    {
        isBallLaunched = launched;
    }
    
    public bool IsBallLaunched()
    {
        return isBallLaunched;
    }
} 