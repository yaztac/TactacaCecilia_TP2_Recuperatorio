using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager scoreManager = other.GetComponent<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(coinValue);
            }


            Destroy(gameObject);
        }
    }
}
