using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int totalScore;

    public int TotalScore
    {
        get => totalScore;
        private set => totalScore = value;
    }

    public void AddValueToScore(int rolledValue, int previousValue)
    {
        TotalScore -= previousValue;
        TotalScore += rolledValue;
        Debug.Log($"Количество очков: {TotalScore}");
    }
}
