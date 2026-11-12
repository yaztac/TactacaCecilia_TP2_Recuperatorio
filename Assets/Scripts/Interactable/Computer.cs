using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private string terminalName = "Terminal #1";
    private bool isHacked = false;

    public void Interact()
    {
        if (!isHacked)
        {
            isHacked = true;
            Debug.Log($"Has hackeado: {terminalName}!");
            //Cambiar de color
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            Debug.Log($"{terminalName} ya esta hackeada.");
        }
    }
}
