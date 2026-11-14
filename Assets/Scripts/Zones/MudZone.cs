using Unity.VisualScripting;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float speedMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("Velocidad reducida");
            player.SetSpeedMultiplier(speedMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ResetSpeedMultiplier(); // Vuelve a velocidad normal
        }
    }
}
