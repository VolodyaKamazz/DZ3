using System;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody diceRigidbody;

    private int rolledValue;

    public ScoreController ScoreController;
    public int RolledValue
    {
        get => rolledValue;
        private set => rolledValue = Mathf.Clamp(value, 1, 6);
    }
    public Rigidbody DiceRigidbody => diceRigidbody;

    private void Awake()
    {
        if (DiceRigidbody == null)
        {
            throw new NullReferenceException($"diceRigidbody у {transform.name} не назначен!");
        }
    }

    public void SetRolledValue(DiceFace face)
    {
        var previousValue = RolledValue;
        switch (face)
        {
            case DiceFace.One:
                RolledValue = 6;
                break;
            case DiceFace.Two:
                RolledValue = 5;
                break;
            case DiceFace.Three:
                RolledValue = 4;
                break;
            case DiceFace.Four:
                RolledValue = 3;
                break;
            case DiceFace.Five:
                RolledValue = 2;
                break;
            case DiceFace.Six:
                RolledValue = 1;
                break;
        }
        Debug.Log($"У {transform.name} установлено значние {RolledValue}");
        ScoreController.AddValueToScore(RolledValue, previousValue);
    }
}
