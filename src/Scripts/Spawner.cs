using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Range(0.1f, 5f)]
    public float spawnRate = 2.0f;
    [Range(0, 20)]
    public int spawnAmount = 1;
    [Range(0f, 30f)]
    public float trajectoryVariance = 15f;
    [Range(-30f, 30f)]
    public float spawnDistance = -15.0f;
    public Asteroid asteroidPrefab;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
            asteroid.transform.parent = GameManager.Instance.Composter;
        }
    }
}