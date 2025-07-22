using System.Collections;
using UnityEngine;

namespace Tools
{
    public class bullet : MonoBehaviour
    {
        public void SetLifeTime(float lifeTime)
        {
            StartCoroutine(KillAfterTime(lifeTime));
        }

        public IEnumerator KillAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}