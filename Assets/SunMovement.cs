using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SunMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    private Light sunLight;
    void Start()
    {
        sunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * speed);
    }
}
