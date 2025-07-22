using UnityEngine;
using UnityEngine.UIElements;

public class PickedUp : MonoBehaviour
{
    private Transform FollowedTransform;
    private float offset;
    private Rigidbody rb;
    private Vector3 lastPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Transform followedTransform)
    {
        FollowedTransform = followedTransform;
        offset = Vector3.Distance(FollowedTransform.position, transform.position);
    } 

    void Update()
    {
        if (FollowedTransform != null)
        {
            rb.linearVelocity = Vector3.zero;
            Vector3 position = Vector3.Lerp(transform.position, FollowedTransform.position + FollowedTransform.forward * offset, 10* Time.deltaTime);
            rb.MovePosition(position);
            lastPos = position;
        }
    }

    public Vector3 GetLastVelocity()
    {
        return lastPos - transform.position;
    }
}
