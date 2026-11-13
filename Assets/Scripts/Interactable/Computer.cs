using Unity.VisualScripting;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [Header("Computer Settings")]
    [SerializeField] private string terminalName = "Terminal #1";
    private bool isHacked = false;

    [Header("Door Reference")]
    [SerializeField] private GameObject[] door;

    public void Interact()
    {
        if (!isHacked)
        {
            isHacked = true;
            Debug.Log($"Has hackeado: {terminalName}!");
            //Cambiar de color
            GetComponent<Renderer>().material.color = Color.green;

            if(door != null)
            {
                foreach (GameObject d in door)
                {
                    d.SetActive(false);
                }
                Debug.Log("La puerta se ha abierto!");
            }
            
        }
        else
        {
            Debug.Log($"{terminalName} ya esta hackeada.");
        }
    }
}
