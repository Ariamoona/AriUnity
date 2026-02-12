using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject obstaclePrefab;

    [Header("Настройки")]
    public float spawnInterval = 2f;  
    public float gapHeight = 3f;       
    public float moveSpeed = 3f;     

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnObstacle();
            timer = spawnInterval;
        }
    }

    void SpawnObstacle()
    {
        float randomY = Random.Range(-1f, 3f);
        Vector3 spawnPos = new Vector3(10f, randomY, 0f);

        GameObject pipe = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        PipeMovement move = pipe.AddComponent<PipeMovement>();
        move.speed = moveSpeed;
    }
}