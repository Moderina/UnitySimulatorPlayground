using TMPro;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject replacePrefab;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private float maxVelocityDurability = 10;
    private Vector3 lastVelocity;

    void Update()
    {
        lastVelocity = GetComponent<Rigidbody>().linearVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            GameObject replacement = Instantiate(replacePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }
        if (Mathf.Abs(lastVelocity.x) > maxVelocityDurability || Mathf.Abs(lastVelocity.y) > maxVelocityDurability || Mathf.Abs(lastVelocity.z) > maxVelocityDurability)
        {
            GameObject replacement = Instantiate(replacePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
