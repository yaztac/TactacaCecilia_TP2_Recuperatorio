using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TMP_Text scoreText;
    public int Score => score;
    private int collectedItems = 0;
    public int CollectedItems => collectedItems;
    private void Start()
    {
        UpdateScoreText();
    }
    public void AddPoints(int points)
    {
        score += points;
        collectedItems++;
        UpdateScoreText();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerStatusChanged();
        }
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Objetos: " + score;
        }
    }
}
