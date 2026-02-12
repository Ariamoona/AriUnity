using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameManager : IStartable
{
    private readonly PlayerController _player;
    private readonly ObstacleSpawner _spawner;
    private readonly ScoreManager _scoreManager;

    [Inject]
    public GameManager(
        PlayerController player,
        ObstacleSpawner spawner,
        ScoreManager scoreManager)
    {
        _player = player;
        _spawner = spawner;
        _scoreManager = scoreManager;
    }

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _player.Reset();
        _spawner.StartSpawning();
        _scoreManager.ResetScore();
    }

    public void GameOver()
    {
        _spawner.StopSpawning();
        Debug.Log($"Game Over! Score: {_scoreManager.CurrentScore}");
    }
}