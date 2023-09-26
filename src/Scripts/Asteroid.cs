using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{
    public Sprite[] Asteroids;
    [Header("Asteroids Settings")]
    [Range(0f, 50f)]
    public float LifeTime = 30f;
    [Range(0f, 100f)]
    public float speed = 50.0f;
    [Range(0f, 5f)]
    public float size = 1.0f;
    [Range(0f, 5f)]
    public float minSize = 0.5f;
    [Range(0f, 5f)]
    public float maxSize = 2f;
    private SpriteRenderer render;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        render.sprite = Asteroids[Random.Range(0, Asteroids.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360f);
        this.transform.localScale = Vector3.one * this.size;
        rb.mass = this.size;
    }

    public void SetTrajectory(Vector2 Trajectory)
    {
        rb.AddForce(Trajectory * this.speed);
        Destroy(this.gameObject, LifeTime);
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bullet")
        {
            if ((this.size * 0.5f) > this.minSize)
            {
                CreateSplit(this.size * 0.5f);
                CreateSplit(this.size * 0.5f);
            }
            Destroy(this.gameObject);
            GameManager.Instance.OnAsteroidDestroy(this);
        }
    }

    private void CreateSplit(float size)
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        Asteroid half = Instantiate(this, position,this.transform.rotation);
        half.size = size;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
        half.transform.parent = GameManager.Instance.Composter;
    }
}
