using UnityEngine;
using System;

public class DiceSpawner : MonoBehaviour
{
    [SerializeField][Min(0)] private int diceCount;
    [SerializeField][Min(0)] private int radius;
    [SerializeField] private bool isEvenly = true;
    [SerializeField] private DiceController dicePrefab;

    public int Radius => radius;
    public int DiceCount => diceCount;
    public DiceController DicePrefab => dicePrefab;
    public bool IsEvenly => isEvenly;
    public ScoreController scoreController;

    public static event Action OnSomethingChanged;

    private void Awake()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<ScoreController>();
        CreateDice();
    }

    private void OnEnable()
    {
        OnSomethingChanged += RecreateDice;
    }

    private void OnDisable()
    {
        OnSomethingChanged -= RecreateDice;
    }

    private void OnValidate()
    {
        OnSomethingChanged?.Invoke();
    }

    private void CreateDice()
    {
        float radianAngleStep;
        if (IsEvenly == true)
        {
            radianAngleStep = 2 * Mathf.PI / diceCount;
        }
        else
        {
            if (Radius == 0)
            {
                radianAngleStep = 0;
            }
            else
            {
                radianAngleStep = 2 * (float)Math.Asin(DicePrefab.gameObject.transform.localScale.x / (2 * Radius));
            }
        }
        transform.position = Vector3.zero;
        for (var i = 0; i < diceCount; i++)
        {
            var x = Radius * Mathf.Cos(radianAngleStep * i);
            var z = Radius * Mathf.Sin(radianAngleStep * i);
            var localPosition = new Vector3(x, transform.position.y + 4, z);
            var cube = Instantiate(DicePrefab, transform);
            cube.transform.SetParent(transform);
            cube.transform.localPosition = localPosition;
            cube.transform.name = $"Dice {i}";
            cube.ScoreController = scoreController;
        }
    }

    private void RecreateDice()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CreateDice();
    }
}