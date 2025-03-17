using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpConfig
    {
        public GameObject powerUpPrefab;
        public float weight = 1f;
    }
    
    [Header("Power-Up Settings")]
    [SerializeField] private PowerUpConfig[] availablePowerUps;
    [SerializeField] private float fallSpeed = 3f;
    
    [Header("Debugging")]
    [SerializeField] private bool debugMode = false;
    
    private float totalWeight;
    
    void Start()
    {
        // Calculate the total weight for weighted random selection
        CalculateTotalWeight();
    }
    
    private void CalculateTotalWeight()
    {
        totalWeight = 0f;
        foreach (PowerUpConfig config in availablePowerUps)
        {
            totalWeight += config.weight;
        }
    }
    
    public void SpawnRandomPowerUp(Vector3 position)
    {
        if (availablePowerUps.Length == 0) return;
        
        // Get a random power-up based on weights
        GameObject selectedPowerUpPrefab = GetRandomWeightedPowerUp();
        
        if (selectedPowerUpPrefab != null)
        {
            // Instantiate the power-up
            GameObject powerUpInstance = Instantiate(selectedPowerUpPrefab, position, Quaternion.identity);
            
            // Set the fall speed
            PowerUp powerUpComponent = powerUpInstance.GetComponent<PowerUp>();
            if (powerUpComponent != null)
            {
                powerUpComponent.SetFallSpeed(fallSpeed);
            }
            
            if (debugMode)
            {
                Debug.Log("Spawned power-up: " + selectedPowerUpPrefab.name);
            }
        }
    }
    
    private GameObject GetRandomWeightedPowerUp()
    {
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        
        foreach (PowerUpConfig config in availablePowerUps)
        {
            cumulativeWeight += config.weight;
            
            if (randomValue <= cumulativeWeight)
            {
                return config.powerUpPrefab;
            }
        }
        
        // Fallback to the first power-up if something goes wrong
        return availablePowerUps[0].powerUpPrefab;
    }
    
    // Helper method to spawn a specific power-up (useful for debugging or special bricks)
    public void SpawnSpecificPowerUp(string powerUpName, Vector3 position)
    {
        foreach (PowerUpConfig config in availablePowerUps)
        {
            if (config.powerUpPrefab.name == powerUpName)
            {
                GameObject powerUpInstance = Instantiate(config.powerUpPrefab, position, Quaternion.identity);
                
                PowerUp powerUpComponent = powerUpInstance.GetComponent<PowerUp>();
                if (powerUpComponent != null)
                {
                    powerUpComponent.SetFallSpeed(fallSpeed);
                }
                
                return;
            }
        }
        
        if (debugMode)
        {
            Debug.LogWarning("Power-up not found: " + powerUpName);
        }
    }
} 