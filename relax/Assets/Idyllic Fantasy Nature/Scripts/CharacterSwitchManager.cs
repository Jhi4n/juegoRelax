using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class CharacterSwitchManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters; // Array de personajes
    [SerializeField] private CinemachineFreeLook[] freeLookCameras; // Array de cámaras FreeLook correspondientes a cada personaje
    [SerializeField] private Canvas characterSelectionCanvas;
    public int currentCharacterIndex = 0; // Índice del personaje actualmente activo

    private void Start()
    {
        // Desactiva todos los personajes excepto el primero al inicio
        for (int i = 0; i < characters.Length; i++)
        {
            var movement = characters[i].GetComponent<PlayerMovement>();
            var npc = characters[i].GetComponent<DogNPC_Random>();
            if (movement != null)
            {
                movement.enabled = (i == currentCharacterIndex);
                npc.enabled = (i!= currentCharacterIndex);
            }
        }

        // Ajusta la prioridad de las cámaras para que la primera tenga la más alta
        UpdateCameraPriorities();
        characterSelectionCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Muestra la interfaz para cambiar de personaje al presionar la tecla 'm'
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleCharacterSelectionCanvas();
        }
    }

    private void ToggleCharacterSelectionCanvas()
    {
        // Activa o desactiva el Canvas según su estado actual
        characterSelectionCanvas.gameObject.SetActive(!characterSelectionCanvas.gameObject.activeSelf);
    }


    private void ShowCharacterSelectionGUI()
    {
        // Ejemplo de código para mostrar GUI de selección de personaje (esto puede variar según la UI que uses)
        for (int i = 0; i < characters.Length; i++)
        {
            if (GUILayout.Button("Personaje " + (i + 1)))
            {
                SwitchCharacter(i);
            }
        }
    }

    public void SwitchCharacter(int newCharacterIndex)
    {
        if (newCharacterIndex != currentCharacterIndex)
        {
            // Desactiva el movimiento del personaje actual
            characters[currentCharacterIndex].GetComponent<PlayerMovement>().enabled = false;
            characters[currentCharacterIndex].GetComponent<DogNPC_Random>().enabled = true;
            characters[currentCharacterIndex].GetComponent<NavMeshAgent>().enabled = true;

            // Cambia al nuevo personaje
            //characters[currentCharacterIndex].SetActive(false);
            currentCharacterIndex = newCharacterIndex;
            characters[currentCharacterIndex].SetActive(true);

            // Activa el movimiento del nuevo personaje
            characters[currentCharacterIndex].GetComponent<DogNPC_Random>().enabled = false;
            characters[currentCharacterIndex].GetComponent<NavMeshAgent>().enabled = false;
            characters[currentCharacterIndex].GetComponent<PlayerMovement>().enabled = true;

            // Actualiza las prioridades de las cámaras
            UpdateCameraPriorities();
        }
    }

    private void UpdateCameraPriorities()
    {
        for (int i = 0; i < freeLookCameras.Length; i++)
        {
            freeLookCameras[i].Priority = (i == currentCharacterIndex) ? 11 : 10;
        }
    }
}
