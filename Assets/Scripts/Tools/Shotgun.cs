using UnityEngine;

namespace Tools
{
    public class Shotgun : MonoBehaviour, ITool
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private float bulletLifeTime = 3;
        
        
        public void PrimaryAction(bool started)
        {
            if (!started) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                firePoint.LookAt(hit.point);
            }
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            bullet.transform.Rotate(Vector3.right, -90);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward.normalized * bulletSpeed, ForceMode.Impulse);
            bullet.GetComponent<bullet>().SetLifeTime(bulletLifeTime);
        }
    }
}