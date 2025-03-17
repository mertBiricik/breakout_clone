using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    [SerializeField] private int startingLives = 3;
    
    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip loseLifeSound;
    [SerializeField] private AudioClip gameOverSound;
    
    private int currentScore = 0;
    private int currentLives;
    private AudioSource audioSource;
    private bool isGameOver = false;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    void Start()
    {
        InitializeGame();
    }
    
    public void InitializeGame()
    {
        currentLives = startingLives;
        currentScore = 0;
        isGameOver = false;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        UpdateUI();
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;
        
        currentScore += points;
        UpdateUI();
    }
    
    public void LoseBall()
    {
        if (isGameOver) return;
        
        currentLives--;
        UpdateUI();
        
        if (audioSource != null && loseLifeSound != null)
        {
            audioSource.PlayOneShot(loseLifeSound);
        }
        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    private void GameOver()
    {
        isGameOver = true;
        
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        Debug.Log("Game Over! Final Score: " + currentScore);
    }
    
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
        
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }
    
    public bool IsGameOver()
    {
        return isGameOver;
    }
    
    public void RestartGame()
    {
        InitializeGame();
        // Find and reset the ball
        BallController[] balls = FindObjectsOfType<BallController>();
        foreach (BallController ball in balls)
        {
            ball.ResetBall();
        }
        
        // Reload the current level
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.ReloadCurrentLevel();
        }
    }
} 