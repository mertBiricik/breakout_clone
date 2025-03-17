using UnityEngine;
using System.Collections;

public class InvertControlsPowerUp : PowerUp
{
    protected override void ApplyEffect()
    {
        // Find the paddle controller
        PaddleController paddleController = FindObjectOfType<PaddleController>();
        
        if (paddleController != null)
        {
            StartCoroutine(ApplyTimedEffect(
                () => InvertControls(paddleController),
                () => RestoreControls(paddleController)
            ));
        }
    }
    
    private void InvertControls(PaddleController paddleController)
    {
        // Add the inverter component to the paddle
        PaddleControlsInverter inverter = paddleController.gameObject.GetComponent<PaddleControlsInverter>();
        if (inverter == null)
        {
            inverter = paddleController.gameObject.AddComponent<PaddleControlsInverter>();
        }
        
        inverter.InvertControls(true);
    }
    
    private void RestoreControls(PaddleController paddleController)
    {
        // Remove the inverter component from the paddle
        PaddleControlsInverter inverter = paddleController.GetComponent<PaddleControlsInverter>();
        if (inverter != null)
        {
            inverter.InvertControls(false);
            Destroy(inverter);
        }
    }
}

// Helper component to invert paddle controls
public class PaddleControlsInverter : MonoBehaviour
{
    private PaddleController paddleController;
    private bool isInverted = false;
    
    private void Awake()
    {
        paddleController = GetComponent<PaddleController>();
    }
    
    private void Update()
    {
        if (isInverted && paddleController != null)
        {
            // Invert the mouse position in screen space
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Screen.width - mousePos.x;
            
            // Override mouse position for the paddle controller
            if (Cursor.visible)
            {
                // Only set the fake position if cursor is visible
                // Using reflection would be better, but for simplicity we'll handle this in a different way
                InvertMouseInput();
            }
            
            // Invert the horizontal axis for keyboard control
            InvertKeyboardInput();
        }
    }
    
    private void InvertMouseInput()
    {
        // Reflect the mouse position about the center of the screen
        float screenMidX = Screen.width / 2f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3 reflectedWorldPos = new Vector3(-worldPos.x, worldPos.y, worldPos.z);
        
        // Override the transform position manually
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(reflectedWorldPos.x, paddleController.GetMinX(), paddleController.GetMaxX());
        transform.position = currentPos;
    }
    
    private void InvertKeyboardInput()
    {
        // Only apply if it's using keyboard controls
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            float moveInput = -Input.GetAxis("Horizontal");
            Vector3 position = transform.position;
            position.x += moveInput * paddleController.GetSpeed() * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, paddleController.GetMinX(), paddleController.GetMaxX());
            transform.position = position;
        }
    }
    
    public void InvertControls(bool invert)
    {
        isInverted = invert;
    }
} 