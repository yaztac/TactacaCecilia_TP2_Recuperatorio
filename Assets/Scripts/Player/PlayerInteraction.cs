using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform raycastOrigin;
    private Vector3 origin;
    private Vector3 direction;
    private RaycastHit hit;
    void Update()
    {
        CheckInteractableObject();
    }

    private void CheckInteractableObject()
    {
        origin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1f;
        direction = transform.forward;

        Debug.DrawRay(origin, direction * interactionDistance, Color.blue);


        if (Physics.Raycast(origin, direction, out hit, interactionDistance, interactableLayer))
        {

            IInteractable interactableObject = hit.collider.GetComponent<IInteractable>();

            if (interactableObject != null)
            {
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactableObject.Interact();
                }
            }
        }
    }
}
