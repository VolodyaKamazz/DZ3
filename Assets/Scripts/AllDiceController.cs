using System;
using System.Collections.Generic;
using UnityEngine;

public class AllDiceController : MonoBehaviour
{
    [SerializeField][Min(0)] private int minForce;
    [SerializeField][Min(1)] private int maxForce;
    [SerializeField][Range(0, 1)] private float torque;
    [SerializeField] private UI canvas;

    private OneDiceController[] allDice;
    private int score;
    private int previousScore;

    public int MinForce => minForce;
    public int MaxForce => maxForce;
    public float Torque => torque;
    public int Score => score;

    private void Awake()
    {
        previousScore = -1;
        allDice = GetComponentsInChildren<OneDiceController>();
    }

    private void FixedUpdate()
    {
        if (IsAtLeastOneDiceMoving())
        {
            score = 0;
            return;
        }
        if (score != previousScore)
        {
            foreach (OneDiceController dice in allDice)
            {
                dice.SetRolledValue();
                score += dice.RolledValue;
            }
            previousScore = score;
            canvas.UpdateText(score);
        }
    }

    public void OnTossButton()
    {
        if (IsAtLeastOneDiceMoving())
        {
            return;
        }
        foreach (OneDiceController dice in allDice)
        {
            dice.Toss();
        }
    }

    public bool IsAtLeastOneDiceMoving() 
    {
        foreach (OneDiceController dice in allDice)
        {
            if (dice.IsMoving())
            {
                return true;
            }
        }
        return false;
    }
}