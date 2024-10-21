using UnityEngine;
using UnityEngine.UI;

public class PlayerTriggerInteraction : MonoBehaviour
{
    public GameObject targetParent;  // El objeto al que el jugador se hará hijo
    public Vector3 targetPosition;   // La posición X, Y, Z a la que moverás al jugador

    public PlayerMovement playerMovement;

    private bool isInTrigger = false;

    public bool cargado = false;

    public Button buttonchihuahua;

    // Detecta si el jugador entra en el Trigger con el tag pastoraleman
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pastoraleman"))
        {
            isInTrigger = true;  // Si el jugador entra en el trigger, activa la bandera
        }
    }

    // Detecta si el jugador sale del Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("pastoraleman"))
        {
            isInTrigger = false;  // Si el jugador sale del trigger, desactiva la bandera
        }
    }

    // Se llama cada frame
    private void Update()
    {
        // Si el jugador está dentro del trigger y presiona la tecla E
        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.enabled = false;
            cargado = true;
            buttonchihuahua.interactable = false;
            // Convierte al jugador en hijo del targetParent
            transform.SetParent(targetParent.transform);

            // Mueve al jugador a la posición deseada
            transform.localPosition = targetPosition;
        }
    }
}
