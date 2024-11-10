using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; } = false;

    [SerializeField] private PlayerController playerController;


    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private int score = 0;

    private void Awake()
    {
        InitializeSingleton();
        ShowStartScreen();
    }

    public void StartGame()
    {
        ResetGame();
        ClearFallingObjects();
        ShowGamePanel();
    }

    public void GameOver()
    {
        if (!IsGameActive) return;

        IsGameActive = false;
        DisplayFinalScore();
        ShowGameOverPanel();
    }

    public void RestartGame() => StartGame();

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void InitializeSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void ResetGame()
    {
        score = 0;
        IsGameActive = true;
        UpdateScoreText();
        playerController.ResetPlayerPostion();
    }

    private void ClearFallingObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FallingObject"))
        {
            Destroy(obj);
        }
    }

    private void ShowGamePanel()
    {
        gamePanel.SetActive(true);
        startScreenPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void ShowStartScreen()
    {
        startScreenPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void ShowGameOverPanel()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void UpdateScoreText() => scoreText.text = $"Score: {score}";

    private void DisplayFinalScore() => finalScoreText.text = $"Final Score: {score}";
}
