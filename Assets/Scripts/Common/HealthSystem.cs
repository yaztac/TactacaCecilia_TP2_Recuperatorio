using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMP_Text healthText;
    private int currentHealth;
    public int CurrentHealth => currentHealth;//usamos getter y setter para el gameManager - solo lectura

    [SerializeField] private bool isDead;
    public bool IsDead
    {
        get => isDead;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthText();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthText();

       
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerStatusChanged();
        }

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }
    private void Dead()
    {
        isDead = true;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerDead();
        }
    }
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Salud: " + currentHealth;
        }
    }
}
