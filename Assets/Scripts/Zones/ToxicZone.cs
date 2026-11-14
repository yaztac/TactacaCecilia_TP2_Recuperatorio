using UnityEngine;
using System.Collections;

public class ToxicZone : MonoBehaviour
{
    [Header("Configuración de Daño Tóxico")]
    [SerializeField] private int damageAmount = 2;
    [SerializeField] private float damageInterval = 1f;

    [Header("Efectos Visuales")]
    [SerializeField] private Renderer zoneRenderer;
    private HealthSystem currentPlayerHealth;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null)
        {
            currentPlayerHealth = health;

            if (!currentPlayerHealth.IsDead && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null && health == currentPlayerHealth)
        {
            StopDamage();
        }
    }

    IEnumerator DamageRoutine()
    {

        if (zoneRenderer != null)
        {
            zoneRenderer.enabled = true;
        }

        Debug.Log("<color=orange>¡Entraste a una zona tóxica!</color>");

        while (currentPlayerHealth != null && !currentPlayerHealth.IsDead)
        {
            yield return new WaitForSeconds(damageInterval);

            if (currentPlayerHealth != null && !currentPlayerHealth.IsDead)
            {
                currentPlayerHealth.TakeDamage(damageAmount);
            }
        }

        StopDamage();
    }

    private void StopDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        if (zoneRenderer != null)
        {
            zoneRenderer.enabled = false;
        }
        currentPlayerHealth = null;
        Debug.Log("<color=green>Saliste de la zona tóxica.</color>");
    }

}