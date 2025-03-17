using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    [Header("Brick Settings")]
    [SerializeField] private int pointValue = 10;
    [SerializeField] private int hitPoints = 1;
    [SerializeField] private float powerUpDropChance = 0.2f;
    [SerializeField] private bool isDestructible = true;
    
    [Header("Visual Feedback")]
    [SerializeField] private Sprite[] damageSprites;
    [SerializeField] private GameObject destroyEffect;
    
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private LevelManager levelManager;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        gameManager = GameManager.Instance;
        levelManager = FindObjectOfType<LevelManager>();
        
        if (isDestructible)
        {
            levelManager.RegisterBrick(this);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleBallCollision();
        }
    }
    
    protected virtual void HandleBallCollision()
    {
        if (!isDestructible) return;
        
        TakeDamage();
    }
    
    public virtual void TakeDamage()
    {
        if (!isDestructible) return;
        
        hitPoints--;
        
        // Update the brick's appearance if it has damage sprites
        if (damageSprites.Length > 0 && hitPoints >= 0 && hitPoints < damageSprites.Length)
        {
            spriteRenderer.sprite = damageSprites[hitPoints];
        }
        
        // If the brick is destroyed
        if (hitPoints <= 0)
        {
            DestroyBrick();
        }
    }
    
    protected virtual void DestroyBrick()
    {
        // Add points to the score
        if (gameManager != null)
        {
            gameManager.AddScore(pointValue);
        }
        
        // Spawn destroy effect if available
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        
        // Chance to drop a power-up
        if (Random.value <= powerUpDropChance)
        {
            SpawnPowerUp();
        }
        
        // Notify the level manager that a brick was destroyed
        if (levelManager != null)
        {
            levelManager.BrickDestroyed(this);
        }
        
        // Destroy the brick
        Destroy(gameObject);
    }
    
    private void SpawnPowerUp()
    {
        PowerUpManager powerUpManager = FindObjectOfType<PowerUpManager>();
        if (powerUpManager != null)
        {
            powerUpManager.SpawnRandomPowerUp(transform.position);
        }
    }
    
    public bool IsDestructible()
    {
        return isDestructible;
    }
    
    // This can be overridden by special bricks to add behaviors on destroy
    protected virtual void OnDestroy()
    {
        // Base brick does nothing special on destroy
    }
} 