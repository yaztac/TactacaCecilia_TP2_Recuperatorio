using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private int score = 0;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI pointsText;
    public int Score
    {
        get => score;
        private set => score = value;
    }

    private void Start()
    {
        UpdateScoreUI();

    }
    
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerStatusChanged();
        }
    }

    private void UpdateScoreUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Monedas " + score.ToString();
        }
    }
}