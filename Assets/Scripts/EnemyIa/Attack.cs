using UnityEngine;
using System.Collections;


public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform sword;
    [SerializeField] private float radius;
    [SerializeField] private Vector3 offsetSword;

    [SerializeField] private int damage = 10;

    [SerializeField] private float cooldownAttack = 1.5f;
    private bool canAttack = true;
    private Vector3 origin;
    private bool isCharacter;

    private void OnDrawGizmos()
    {
        if (sword != null)
        {
            Vector3 gizmoOrigin = sword.TransformPoint(offsetSword);
            Gizmos.color = isCharacter ? Color.green : Color.red;
            Gizmos.DrawWireSphere(gizmoOrigin, radius);
        }
    }

    public void Melee()
    {
        if (!canAttack) return;

        CalculateOrigin(offsetSword);

        Collider[] colliders = Physics.OverlapSphere(origin, radius);
        if (colliders.Length > 0)//solo para visualizar el debug
        {
            foreach (Collider col in colliders)
            {
                // El enemigo busca específicamente al jugador
                if (col.gameObject.CompareTag("Player"))
                {
                    ApplyDamage(col.gameObject);
                    isCharacter = true;
                    break; // Rompe el bucle al impactar al jugador
                }
                else
                {
                    isCharacter = false;
                }

            }
        }
        else
        {
            isCharacter = false;
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldownAttack);
        canAttack = true;
    }

    private void ApplyDamage(GameObject hitObj)
    {
        HealthSystem health = hitObj.GetComponent<HealthSystem>();
        if (health != null && hitObj.CompareTag("Player"))
        {
            health.TakeDamage(damage);
            isCharacter = true;
            Debug.Log($"Enemigo hizo {damage} de daño al jugador.");
        }
        else
        {
            isCharacter = false;
        }

    }

    private void CalculateOrigin(Vector3 offset)
    {
        if (sword != null)
        {
            origin = sword.TransformPoint(offset);
        }
        else
        {
            // Si no se asignó un objeto sword, toma la posición del propio enemigo con el offset
            origin = transform.position + transform.TransformDirection(offset);
        }
    }

}
