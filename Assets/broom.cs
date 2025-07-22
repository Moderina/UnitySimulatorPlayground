using System.Collections;
using Tools;
using UnityEngine;

public class broom : MonoBehaviour, ITool
{
    private bool playAnimation = false;

    private int animationOffset = 0;

    void Update()
    {
        var rotX = Camera.main.transform.rotation.eulerAngles.x + 32 + animationOffset;
        transform.localRotation = Quaternion.Euler(-rotX, -10, 0);
    }

    public void PrimaryAction(bool started)
    {
        if (started)
        {
            if (playAnimation) return;
            playAnimation = true;
            StartCoroutine(BroomAnimation());
            StartCoroutine(CleanUnderMouse());
        }
        else 
            playAnimation = false;
    }

    private IEnumerator BroomAnimation()
    {
        var InitRot = transform.localRotation;

        while (playAnimation)
        {
            var maxRotX = 18;
            Quaternion end = InitRot * Quaternion.Euler(Vector3.right * maxRotX) ;

            float time = 0.5f;
            float elapsed = 0;

            for (int i = 0; i < maxRotX; i++)
            {
                animationOffset = i;
                yield return null;
            }
            for (int i = maxRotX; i > -maxRotX; i--)
            {
                animationOffset = i;
                yield return null;
            }
            for (int i = -maxRotX; i < 0; i++)
            {
                animationOffset = i;
                yield return null;
            }
            animationOffset = 0;
        }
    }

    private IEnumerator CleanUnderMouse()
    {
        Debug.Log("cleaning");
        Camera cam = Camera.main;
        Ray ray;
        GameObject hitObject = null;
        //TODO: make it scriptableobject data
        float time = 2f;
        float elapsed = 0;
        while (playAnimation)
        {
            if (!hitObject)
            {
                Debug.Log("no object");
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit currentHit, 2, 1 << 8))
                {
                    hitObject = currentHit.transform.gameObject;
                    Debug.Log("but new found");
                }
                else
                {
                    Debug.Log("and nothing found");
                    yield return null;
                    continue;
                }
            }
            elapsed += Time.deltaTime;
            Vector3 directionToTarget = (hitObject.transform.position - cam.transform.position).normalized;
            float dot = Vector3.Dot(cam.transform.forward, directionToTarget);
            if (dot < 0.98f)
            {
                Debug.Log("mosue moved");
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit newHit, 2, 1 << 8))
                {
                    if (hitObject == newHit.transform.gameObject)
                    {
                        yield return null;
                        continue;
                    }
                    elapsed = 0;
                    hitObject = newHit.transform.gameObject;
                }
                else
                {
                    elapsed = 0;
                    hitObject = null;
                    yield return null;
                    continue;
                }
            }
            if (elapsed >= time && hitObject)
            {
                Debug.Log("time ended");
                elapsed = 0;
                Destroy(hitObject);
                hitObject = null;
            }
            yield return null;
        }

    }
}
