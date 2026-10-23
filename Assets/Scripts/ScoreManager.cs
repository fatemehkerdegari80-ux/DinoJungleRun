
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Player")]
    public PlayerDash playerDash;
    public PlayerHealth playerHealth;

    [Header("Score UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    [Header("Settings")]
    public float pointsPerUnit = 1f;

    private float distanceTraveled;
    private int coinPoints;
    private int lastScore = -1;
    private bool savedOnDeath;

    private const string HighScoreKey = "DinoHighScore";

    public int CurrentScore
    {
        get
        {
            int runningScore =
                Mathf.FloorToInt(
                    distanceTraveled * pointsPerUnit
                );

            return runningScore + coinPoints;
        }
    }

    public int HighScore { get; private set; }

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void Start()
    {
        RefreshScore();
    }

    private void Update()
    {
        if (playerDash == null || playerHealth == null)
            return;

        if (playerHealth.CurrentLives <= 0)
        {
            if (!savedOnDeath)
            {
                PlayerPrefs.Save();
                savedOnDeath = true;
            }

            return;
        }

        distanceTraveled +=
            playerDash.CurrentSpeed * Time.deltaTime;

        if (CurrentScore != lastScore)
        {
            RefreshScore();
        }
    }

    public void AddCoin()
    {
        if (playerHealth == null ||
            playerHealth.CurrentLives <= 0)
            return;

        coinPoints += 5;

        RefreshScore();

        Debug.Log("Coin collected! +5 points");
    }

    private void RefreshScore()
    {
        int score = CurrentScore;

        if (score > HighScore)
        {
            HighScore = score;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
        }

        if (scoreText != null)
            scoreText.text = "SCORE: " + score;

        if (highScoreText != null)
            highScoreText.text = "BEST: " + HighScore;

        lastScore = score;
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
            PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
