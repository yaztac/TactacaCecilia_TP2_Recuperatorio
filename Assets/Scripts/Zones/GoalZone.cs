using UnityEngine;
using System.Collections;

public class GoalZone : MonoBehaviour
{
    private bool reached;

    [SerializeField] private GameObject[] vfx;
    [SerializeField] private float celebrationDelay = 3f;
    private void OnTriggerEnter(Collider other)
    {
        if (reached) return;
        if (other.GetComponentInParent<PlayerMovement>() == null) return;
        if (GameManager.Instance == null) return;

        reached = true;

        StartCoroutine(VictoryCelebrationRoutine());

    }

    // Vfx cuando llega a meta se activan, espera unos segundos y pasa al siguiente nivel
    IEnumerator VictoryCelebrationRoutine()
    {
        foreach (var v in vfx)
        {
            v.SetActive(true);
        }
        yield return new WaitForSeconds(celebrationDelay);
        GameManager.Instance.NotifyGoalZone();
    }
}
