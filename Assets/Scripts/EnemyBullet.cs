using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 3f; // Time before the bullet destroys itself
    public string enemyTag = "Enemy"; // Tag for enemies
    public string bulletTag = "bullet"; // Tag for bullets
    public string enemybulletTag = "enemybullet"; // Tag for bullets

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy after a set time
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            return; // Ignore enemies
        }

        if (collision.gameObject.CompareTag(bulletTag))
        {
            return; // Ignore enemies
        }

        if (collision.gameObject.CompareTag(enemybulletTag))
        {
            return; // Ignore enemies
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return; // Ignore player objects
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return; // Ignore ground objects
        }

        Destroy(gameObject); // Destroy on collision with other objects
    }
}
