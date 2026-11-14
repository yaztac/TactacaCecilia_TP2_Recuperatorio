using UnityEngine;
using System.Collections;

public class HealthZone : MonoBehaviour
{
    [Header("Configuración de Curación")]
    [SerializeField] private int healAmount = 20;
    [SerializeField] private float healInterval = 1.5f;
    [SerializeField] private float cooldownDuration = 5f;

    [Header("Efectos Visuales")]
    [SerializeField] private GameObject vfx;

    private bool isDisabled = false;
    private HealthSystem currentPlayerHealth;
    private Coroutine healingCoroutine;

    //Color 
    private Renderer renderer;
    private Color originalColor;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDisabled) return;

        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null)
        {
            currentPlayerHealth = health;

            if (currentPlayerHealth.CurrentHealth < currentPlayerHealth.MaxHealth)
            {
                if (healingCoroutine == null)
                {
                    healingCoroutine = StartCoroutine(HealingRoutine());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null && health == currentPlayerHealth)
        {
            StopHealing();
        }
    }

    IEnumerator HealingRoutine()
    {
        vfx.SetActive(true);
        while (currentPlayerHealth != null && currentPlayerHealth.CurrentHealth < currentPlayerHealth.MaxHealth)
        {
            yield return new WaitForSeconds(healInterval);

            if (currentPlayerHealth != null)
            {
                currentPlayerHealth.Heal(healAmount);
                if (currentPlayerHealth.CurrentHealth >= currentPlayerHealth.MaxHealth)
                {
                    Debug.Log("<color=yellow>¡El jugador alcanzó los 100 de salud! Deshabilitando la zona de curación temporalmente...</color>");
                    DisableZone();
                    yield break;
                }
            }
        }
        StopHealing();

    }

    private void StopHealing()
    {
        if (healingCoroutine != null)
        {
            StopCoroutine(healingCoroutine);
            healingCoroutine = null;
        }
        vfx.SetActive(false);
        currentPlayerHealth = null;
    }

    private void DisableZone()
    {
        StopHealing();
        isDisabled = true;
        vfx.SetActive(false);
        renderer.material.color = Color.white;

        // Iniciamos el tiempo de descanso de la zona
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownDuration);

        // Volvemos a habilitar la zona
        isDisabled = false;
        renderer.material.color = originalColor;
        Debug.Log("<color=green>La zona de salud se ha vuelto a habilitar.</color>");
    }
}
