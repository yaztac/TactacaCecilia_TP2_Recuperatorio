using UnityEngine;

public class PlayerRaycastAttack : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float rayDistance = 15f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float stunDuration = 5f;
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootRaycastAttack();
        }
    }

    void ShootRaycastAttack()
    {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = firePoint != null ? firePoint.forward : Camera.main.transform.forward;

        Debug.DrawRay(origin, direction * rayDistance, Color.cyan, 1.5f);

        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, rayDistance, enemyLayer))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.Stun(stunDuration);
                Debug.Log("¡Ataque paralizador exitoso!");
            }
        }
    }

}
