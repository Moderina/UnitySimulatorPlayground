using System.Linq;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private PickedUp pickedUpScript;
    void Start()
    {
        pickedUpScript = gameObject.AddComponent<PickedUp>();
        pickedUpScript.enabled = false;
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
        pickedUpScript.enabled = true;
        pickedUpScript.SetUp(playerPickUpPoint);
    }

    public void OnPutDown()
    {
        Vector3 velocityVector = pickedUpScript.GetLastVelocity();
        float Speed = 10;
        velocityVector = new Vector3(velocityVector.x, velocityVector.y * Speed, velocityVector.z);
        rb.useGravity = true;
        pickedUpScript.enabled = false;
        rb.linearVelocity = velocityVector;
    }

    public void OnThrown(Vector3 direction)
    {
        Debug.Log(direction);
        direction.Normalize();
        rb.AddForce(direction * 10, ForceMode.Impulse);
    }
}
