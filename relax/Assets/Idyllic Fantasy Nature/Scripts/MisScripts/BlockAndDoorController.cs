using UnityEngine;

public class BlockAndDoorController : MonoBehaviour
{
    public GameObject door;            // Referencia a la puerta
    public Transform block1;           // Referencia al bloque 1
    public Transform block2;           // Referencia al bloque 2
    public float blockMoveAmount = 0.5f; // Cantidad que baja el bloque
    public float smoothSpeed = 2f;     // Velocidad de la transición suave

    private Vector3 block1StartPos;    // Posición inicial del bloque 1
    private Vector3 block2StartPos;    // Posición inicial del bloque 2
    private Vector3 doorStartPos;      // Posición inicial de la puerta
    private Vector3 doorOpenPos;       // Posición abierta de la puerta

    private int dogCount = 0;          // Contador de perros sobre el bloque
    private bool isPastorOnBlock = false; // Bandera para saber si el pastor alemán está en el bloque
    private bool isBlockDown = false;     // Estado actual de los bloques

    void Start()
    {
        // Guardamos las posiciones iniciales
        block1StartPos = block1.position;
        block2StartPos = block2.position;
        doorStartPos = door.transform.position;

        // Definimos la posición de la puerta cuando está abierta
        doorOpenPos = doorStartPos + new Vector3(0, 2f, 0); // Por ejemplo, abre 2 unidades hacia arriba
    }

    void Update()
    {
        // Si el bloque debe estar abajo, lo bajamos suavemente
        if (isBlockDown)
        {
            MoveBlock(block1, block1StartPos - new Vector3(0, blockMoveAmount, 0));
            MoveBlock(block2, block2StartPos - new Vector3(0, blockMoveAmount, 0));
            MoveDoor(door, doorOpenPos);
        }
        // Si no, los bloques regresan a su posición original
        else
        {
            MoveBlock(block1, block1StartPos);
            MoveBlock(block2, block2StartPos);
            MoveDoor(door, doorStartPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pastoraleman"))
        {
            isPastorOnBlock = true;
            isBlockDown = true; // Baja los bloques y abre la puerta
        }
        else if (collision.CompareTag("Dog"))
        {
            dogCount++;
            if (dogCount >= 2 && !isPastorOnBlock)
            {
                isBlockDown = true; // Baja los bloques si hay 2 perros que no son el Pastor Alemán
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PastorAleman"))
        {
            isPastorOnBlock = false;
            isBlockDown = dogCount >= 2; // Solo sube si no hay 2 perros
        }
        else if (collision.CompareTag("Dog"))
        {
            dogCount--;
            if (dogCount < 2 && !isPastorOnBlock)
            {
                isBlockDown = false; // Sube los bloques si hay menos de 2 perros
            }
        }
    }

    void MoveBlock(Transform block, Vector3 targetPosition)
    {
        // Mueve el bloque suavemente hacia la posición objetivo
        block.position = Vector3.Lerp(block.position, targetPosition, Time.deltaTime * smoothSpeed);
    }

    void MoveDoor(GameObject door, Vector3 targetPosition)
    {
        // Mueve la puerta suavemente hacia la posición objetivo
        door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
