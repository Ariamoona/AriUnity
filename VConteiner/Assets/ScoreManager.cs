public class ScoreManager
{
    public int CurrentScore { get; private set; }

    public void AddScore(int points)
    {
        CurrentScore += points;
        UnityEngine.Debug.Log($"Score: {CurrentScore}");
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}