using UnityEngine;
using System.Collections;

public abstract class PowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    [SerializeField] protected float duration = 10f;
    [SerializeField] protected AudioClip activationSound;
    [SerializeField] protected GameObject activationEffect;
    
    private float fallSpeed = 3f;
    private Rigidbody2D rb;
    
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // No physics gravity, we'll control the falling
        }
    }
    
    protected void Start()
    {
        // Destroy the power-up after 10 seconds if not collected
        Destroy(gameObject, 10f);
    }
    
    protected void Update()
    {
        // Move the power-up downward
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
    
    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            // Power-up collected
            OnCollected();
        }
        else if (other.CompareTag("DeathZone"))
        {
            // Power-up fell off screen
            Destroy(gameObject);
        }
    }
    
    protected void OnCollected()
    {
        // Play sound effect
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, transform.position);
        }
        
        // Spawn visual effect
        if (activationEffect != null)
        {
            Instantiate(activationEffect, transform.position, Quaternion.identity);
        }
        
        // Apply power-up effect
        ApplyEffect();
        
        // Destroy the power-up object
        Destroy(gameObject);
    }
    
    // Abstract method to be implemented by specific power-ups
    protected abstract void ApplyEffect();
    
    // Helper method for timed power-ups
    protected IEnumerator ApplyTimedEffect(System.Action applyEffect, System.Action removeEffect)
    {
        applyEffect.Invoke();
        
        yield return new WaitForSeconds(duration);
        
        removeEffect.Invoke();
    }
} 