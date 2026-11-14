using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;//usamos getter y setter para el gameManager - solo lectura
    public int MaxHealth => maxHealth; 

    [SerializeField] private bool isDead;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = 60;
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

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthText();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerStatusChanged();
        }

        Debug.Log("<color=green>Jugador curado. Salud actual: " + currentHealth + "</color>");
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
