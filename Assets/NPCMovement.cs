using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    int maxWaitingTime = 5;
    private Coroutine waitAtCounter;
    void Start()
    {
        // MoveTo(new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50)));
        target = GameObject.FindGameObjectWithTag("Counter");
        MoveTo(target.GetComponent<Counter>().GetInteractionPoint());
        StartCoroutine(IfDestinationReached());
    }

    void MoveTo(Vector3 destination)
    {
        agent.destination = destination;
    }

    IEnumerator IfDestinationReached()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return new WaitForSeconds(1f);
        }
        WhenDestinationReached();
    }

    void WhenDestinationReached()
    {
        target.GetComponent<Counter>().Interact();
    }
}
