using UnityEngine;

public class ArmoredBrick : Brick
{
    [Header("Armored Brick Settings")]
    [SerializeField] private int maxHitPoints = 3;
    [SerializeField] private Color[] armorColors;
    
    private SpriteRenderer spriteRenderer;
    private int currentHitPoints;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHitPoints = maxHitPoints;
    }
    
    void Start()
    {
        // Initialize appearance based on hitpoints
        UpdateAppearance();
    }
    
    public override void TakeDamage()
    {
        currentHitPoints--;
        
        // Update the appearance
        UpdateAppearance();
        
        // If completely destroyed
        if (currentHitPoints <= 0)
        {
            base.TakeDamage(); // This will handle the actual destruction
        }
        else
        {
            // Play sound or effect for a hit but not destruction
            PlayHitEffect();
        }
    }
    
    private void UpdateAppearance()
    {
        if (armorColors.Length > 0 && currentHitPoints > 0)
        {
            // Calculate the appropriate color index
            int colorIndex = Mathf.Clamp(maxHitPoints - currentHitPoints, 0, armorColors.Length - 1);
            spriteRenderer.color = armorColors[colorIndex];
        }
    }
    
    private void PlayHitEffect()
    {
        // Play a sound or visual effect when the brick is hit but not destroyed
        // Could add particle effects, sound, or animation here
        transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        
        // Use DOTween or a coroutine to restore scale
        StartCoroutine(RestoreScale());
    }
    
    private System.Collections.IEnumerator RestoreScale()
    {
        yield return new WaitForSeconds(0.1f);
        transform.localScale = Vector3.one;
    }
} 