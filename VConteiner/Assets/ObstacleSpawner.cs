using System.Collections;
using UnityEngine;

public class ObstacleSpawner
{
    private readonly GameObject _obstaclePrefab;
    private bool _isSpawning = false;
    private MonoBehaviour _coroutineRunner;

    public ObstacleSpawner(GameObject obstaclePrefab)
    {
        _obstaclePrefab = obstaclePrefab;
        var go = new GameObject("SpawnerRunner");
        _coroutineRunner = go.AddComponent<CoroutineRunner>();
    }

    public void StartSpawning()
    {
        _isSpawning = true;
        _coroutineRunner.StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }

    private IEnumerator SpawnRoutine()
    {
        while (_isSpawning)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(2f);
        }
    }

    private void SpawnObstacle()
    {
        var randomY = Random.Range(-2f, 2f);
        var position = new Vector3(10f, randomY, 0);
        Object.Instantiate(_obstaclePrefab, position, Quaternion.identity);
    }

    private class CoroutineRunner : MonoBehaviour { }
}