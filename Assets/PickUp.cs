using System.Linq;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleOutlineMaterial(Material material = null)
    {
        var mats = GetComponent<Renderer>().materials;
        if (material)
        {
            gameObject.layer = LayerMask.NameToLayer("Outline");
            Material[] newMats = new Material[mats.Length + 1];
            mats.CopyTo(newMats, 0);
            newMats[mats.Length] = material;
            GetComponent<Renderer>().materials = newMats;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Interact");
            GetComponent<Renderer>().materials = mats.Take(mats.Count() - 1).ToArray();
        }
    }

    public void OnPickedUp(Transform playerPickUpPoint)
    {
        rb.useGravity = false;
        Debug.Log("Picked up");
        // transform.position = playerPickUpPoint.position;
        transform.parent = playerPickUpPoint;
        // rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void OnPutDown(Vector3 lastPos)
    {
        Vector3 velocityVector = transform.position - lastPos;
        float Speed = 10;
        velocityVector = new Vector3(velocityVector.x, velocityVector.y * Speed, velocityVector.z);
        // rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        transform.parent = null;
        rb.linearVelocity = velocityVector;
    }

    public void OnThrown(Vector3 direction)
    {
        Debug.Log(direction);
        direction.Normalize();
        rb.AddForce(direction * 10, ForceMode.Impulse);
    }
}
