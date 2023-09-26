using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Range(0f, 500f)]
    public float speed = 250.0f;
    [Range(0f, 5f)]
    public float LifeTime = 1f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rb.AddForce(direction * speed);
        Destroy(this.gameObject, this.LifeTime);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        Destroy(this.gameObject);
    }
}
