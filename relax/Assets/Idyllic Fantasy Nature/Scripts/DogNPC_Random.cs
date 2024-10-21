using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DogNPC_Random : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;  // Componente NavMeshAgent del perro
    public float patrolRadius = 10f;    // Radio en el que el perro puede moverse de manera aleatoria
    public float waitTime = 2f;         // Tiempo que espera el perro antes de moverse a otro punto

    private bool isWaiting = false;     // Indica si el perro está esperando antes de moverse de nuevo
    private bool canDetectInput = false;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination();  // Al iniciar, establece un destino aleatorio
        StartCoroutine(DelayInputDetection(3f)); 
    }

    IEnumerator DelayInputDetection(float delay)
        {
            yield return new WaitForSeconds(delay);  // Wait for 'delay' seconds
            canDetectInput = true;  // Allow input detection after delay
        }

    void Update()
    {
        // Si el perro no tiene un camino pendiente y ha llegado a su destino
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Si no está esperando, comienza a esperar antes de moverse de nuevo
            if (!isWaiting)
            {
                StartCoroutine(WaitBeforeNextMove());
            }
        }
    }

    // Establece un nuevo destino aleatorio dentro del radio de patrullaje
    private void SetNewRandomDestination()
    {
        // Generar una dirección aleatoria dentro de un área esférica
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position; // Asegurarse de que el punto esté alrededor del perro

        NavMeshHit navHit;
        // Intentar encontrar una posición válida en el NavMesh cerca de la dirección generada
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(navHit.position); // Establecer la nueva posición como destino
        }
    }

    // Corrutina que espera antes de establecer el próximo destino
    private IEnumerator WaitBeforeNextMove()
    {
        isWaiting = true;  // Indicar que está esperando
        yield return new WaitForSeconds(waitTime);  // Esperar por el tiempo configurado
        SetNewRandomDestination();  // Establecer un nuevo destino
        isWaiting = false;  // Dejar de esperar
    }
    private void OnDrawGizmos()
    {
        // Cambiar el color de los Gizmos a amarillo
        Gizmos.color = Color.yellow;

        // Dibujar una esfera que representa el radio de patrullaje
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
