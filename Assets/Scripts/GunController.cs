using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public Transform firePoint; // The transform from which bullets will be fired
    public float bulletSpeed = 10f;

    void Update()
    {
        RotateTowardsMouse();

        if (Input.GetButtonDown("Fire1")) // Default left mouse button or controller equivalent
        {
            Shoot();
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("Fire point is not assigned! Assign a fire point in the inspector.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed; // Adjust for 2D side-scrolling
        }
    }
}