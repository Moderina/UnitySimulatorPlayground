using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    void Start()
    {
        
    }
    
    public Vector3 GetInteractionPoint() { return interactionPoint.position; } 

    public void Interact()
    {
        Debug.Log("counter reached");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
