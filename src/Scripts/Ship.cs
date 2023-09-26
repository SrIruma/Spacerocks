#pragma warning disable
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("Ship Settings")]
    [Range(0f, 20f)]
    public float moveSpeed = 5.0f;
    [Range(0f, 20f)]
    public float rotateSpeed = 10.0f;
    [Header("Bullets Settings")]
    public Bullet bulletPrefab;
    public Transform bulletArea;
    private bool thrusting;
    private float turnDirection;
    private Rigidbody2D rb;
    public AudioClip shootSound;
    private bool isCoolingDown = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else { turnDirection = 0.0f; }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(this.transform.up * this.moveSpeed);
        }
        if(turnDirection != 0.0f)
        {
            rb.AddTorque(turnDirection * this.rotateSpeed);
        }
    }
    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, bulletArea.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
        bullet.transform.parent = GameManager.Instance.Composter;
        if (!GameManager.Instance.PauseScreen.active)
        {
            AudioSource.PlayClipAtPoint(shootSound, this.transform.position);
        }
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Asteroid")
        {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);
            GameManager.Instance.OnPlayerKilled();
        }
    }
}
#pragma warning restore