using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DetachObject : MonoBehaviour
{
    public Transform childObject;  // El objeto que actualmente es hijo y queremos desvincular
    private bool hasChild = false;

    public PlayerTriggerInteraction chihuahua;
    public Button buttonchihuahua;


    // Se llama cada frame
    void Update()
    {
        // Verifica si el personaje tiene un hijo y si presiona la tecla E
        if (chihuahua.cargado && Input.GetKeyDown(KeyCode.E))
        {
            // Desvincula el objeto hijo
            DetachChild();
            chihuahua.GetComponent<DogNPC_Random>().enabled = true;
            chihuahua.GetComponent<NavMeshAgent>().enabled = true;
            chihuahua.cargado = false;
            buttonchihuahua.interactable = true;
        }
    }

    // Método para desvincular al hijo actual
    private void DetachChild()
    {
        if (childObject != null)
        {
            // Desvincula el objeto, dejándolo sin padre en la jerarquía
            childObject.SetParent(null);
            childObject = null;
            hasChild = false;
        }
    }
}
