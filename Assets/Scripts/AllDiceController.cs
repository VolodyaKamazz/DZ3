using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AllDiceController : MonoBehaviour
{
    [SerializeField][Min(0)] private int minForce;
    [SerializeField][Min(1)] private int maxForce;
    [SerializeField][Range(0, 1)] private float torque;
    [SerializeField] private OneDiceController[] allDice;

    private int score;
    private int movingDices;
    private OneDiceController[] AllDice => allDice;

    public int MinForce => minForce;
    public int MaxForce => maxForce;
    public float Torque => torque;
    public int MovingDices
    {
        get => movingDices;
        private set
        {
            movingDices = value;
            if (value == 0)
            {
                SetScore();
                OnScoreChanged?.Invoke(score);
            }
        }
    }

    public UnityEvent<int> OnScoreChanged;

    public void OnTossButton()
    {
        if (IsAtLeastOneDiceMoving())
        {
            return;
        }
        MovingDices = AllDice.Count();
        score = 0;
        OnScoreChanged?.Invoke(score);
        foreach (OneDiceController dice in AllDice)
        {
            dice.Toss();
        }
    }

    public bool IsAtLeastOneDiceMoving()
    {
        foreach (OneDiceController dice in AllDice)
        {
            if (dice.IsMoving())
            {
                return true;
            }
        }
        return false;
    }

    private void SetScore()
    {
        foreach (var dice in AllDice)
        {
            dice.SetRolledValue();
            score += dice.RolledValue;
        }
    }

    private void ReduceMovingDices()
    {
        MovingDices -= 1;
    }

    private void OnEnable()
    {
        foreach (var dice in AllDice)
        {
            dice.OnDiceLand.AddListener(ReduceMovingDices);
        }
    }

    private void OnDisable()
    {
        foreach (var dice in AllDice)
        {
            dice.OnDiceLand.RemoveListener(ReduceMovingDices);
        }
    }
}