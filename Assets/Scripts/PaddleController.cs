using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float boundaryPadding = 1f;
    [SerializeField] private bool useMouseControl = true;

    private float minX, maxX;
    private float paddleHalfWidth;
    private Camera mainCamera;
    private Vector3 originalScale;

    void Start()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;
        paddleHalfWidth = transform.localScale.x / 2f;
        CalculateBoundaries();
    }

    void Update()
    {
        if (useMouseControl)
        {
            MouseControl();
        }
        else
        {
            KeyboardControl();
        }
    }

    void CalculateBoundaries()
    {
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        minX = -screenBounds.x + paddleHalfWidth + boundaryPadding;
        maxX = screenBounds.x - paddleHalfWidth - boundaryPadding;
    }

    void KeyboardControl()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 position = transform.position;
        position.x += moveInput * speed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
    }

    void MouseControl()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(worldPosition.x, minX, maxX);
        transform.position = position;
    }

    public void ToggleControl()
    {
        useMouseControl = !useMouseControl;
    }
    
    // Methods for power-ups to use
    public void StoreOriginalScale()
    {
        originalScale = transform.localScale;
    }
    
    public void RestoreOriginalScale()
    {
        transform.localScale = originalScale;
        // Update paddle half width with the restored scale
        paddleHalfWidth = transform.localScale.x / 2f;
        // Recalculate boundaries based on new size
        CalculateBoundaries();
    }
    
    // Called when the paddle's scale changes (used by power-ups)
    public void UpdatePaddleSize()
    {
        paddleHalfWidth = transform.localScale.x / 2f;
        CalculateBoundaries();
    }
    
    // Paddle speed methods for power-ups
    public float GetSpeed()
    {
        return speed;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    // Boundary accessors for power-ups
    public float GetMinX()
    {
        return minX;
    }
    
    public float GetMaxX()
    {
        return maxX;
    }
} 