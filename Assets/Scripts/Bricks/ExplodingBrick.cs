using UnityEngine;
using System.Collections;

public class ExplodingBrick : Brick
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private LayerMask brickLayer;
    
    protected override void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return; // Skip if scene is unloading
        
        // Create explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
        
        // Find all bricks within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, brickLayer);
        
        foreach (Collider2D collider in colliders)
        {
            // Skip the exploding brick itself
            if (collider.gameObject == gameObject) continue;
            
            // Damage the nearby brick
            Brick brick = collider.GetComponent<Brick>();
            if (brick != null)
            {
                brick.TakeDamage();
            }
        }
        
        // Call the base implementation
        base.OnDestroy();
    }
    
    // For debug visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
} 