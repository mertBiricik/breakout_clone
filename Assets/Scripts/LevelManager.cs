using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private float levelTransitionDelay = 2f;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private AudioClip levelCompleteSound;
    
    [Header("Level References")]
    [SerializeField] private GameObject[] levelPrefabs;
    
    private List<Brick> activeBricks = new List<Brick>();
    private GameObject currentLevelInstance;
    private AudioSource audioSource;
    private GameManager gameManager;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    void Start()
    {
        gameManager = GameManager.Instance;
        LoadLevel(currentLevelIndex);
        
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }
    
    public void RegisterBrick(Brick brick)
    {
        if (!activeBricks.Contains(brick))
        {
            activeBricks.Add(brick);
        }
    }
    
    public void BrickDestroyed(Brick brick)
    {
        activeBricks.Remove(brick);
        
        // Check if level is complete
        if (activeBricks.Count == 0)
        {
            StartCoroutine(HandleLevelComplete());
        }
    }
    
    private IEnumerator HandleLevelComplete()
    {
        // Play level complete sound
        if (audioSource != null && levelCompleteSound != null)
        {
            audioSource.PlayOneShot(levelCompleteSound);
        }
        
        // Show level complete UI if available
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }
        
        // Wait for the transition delay
        yield return new WaitForSeconds(levelTransitionDelay);
        
        // Hide level complete UI
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
        
        // Advance to next level
        int nextLevelIndex = currentLevelIndex + 1;
        
        // Check if there are more levels
        if (nextLevelIndex < levelPrefabs.Length)
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {
            // No more levels, game complete
            Debug.Log("Game Complete! All levels finished.");
            // TODO: Show game complete UI
        }
    }
    
    public void LoadLevel(int levelIndex)
    {
        // Make sure the level index is valid
        if (levelIndex < 0 || levelIndex >= levelPrefabs.Length)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }
        
        // Clean up previous level if it exists
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }
        
        // Clear active bricks list
        activeBricks.Clear();
        
        // Instantiate the new level
        if (levelPrefabs[levelIndex] != null)
        {
            currentLevelInstance = Instantiate(levelPrefabs[levelIndex], Vector3.zero, Quaternion.identity);
            currentLevelIndex = levelIndex;
        }
        else
        {
            Debug.LogError("Level prefab is null for index: " + levelIndex);
        }
        
        // Reset the ball
        BallController[] balls = FindObjectsOfType<BallController>();
        foreach (BallController ball in balls)
        {
            ball.ResetBall();
        }
    }
    
    public void ReloadCurrentLevel()
    {
        LoadLevel(currentLevelIndex);
    }
    
    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < levelPrefabs.Length)
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("No more levels available.");
        }
    }
} 