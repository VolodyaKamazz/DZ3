using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AllDiceController : MonoBehaviour
{
    [SerializeField] private int minForce;
    [SerializeField] private int maxForce;
    [SerializeField] private float torque;
    [SerializeField] private OneDiceController prefab;
    [SerializeField] private List<OneDiceController> allDice;

    private int score;
    private int movingDicesCount;
    private int diceCount;
    private int winPoints;
    private int drawPoints;
    private int maxScore;
    private Vector3[,] spawnPoints;
    private bool isSpawnCoroutineStarted;

    public List<OneDiceController> AllDice => allDice;
    public float Torque => torque;
    public int Score => score;
    public int MinForce => minForce;
    public int MaxForce => maxForce;
    public int WinPoints => winPoints;
    public int DrawPoints => drawPoints;
    public int DiceCount
    {
        get => diceCount;
        private set
        {
            diceCount = Mathf.Max(value, 1);
        }
    }
    public int MovingDicesCount
    {
        get => movingDicesCount;
        private set
        {
            movingDicesCount = value;
            if (value == 0)
            {
                SetScore();
                OnScoreChanged?.Invoke();
            }
        }
    }

    public UnityEvent OnScoreChanged;
    public UnityEvent OnDiceCountChange;

    private void Awake()
    {
        spawnPoints = GetSpawnMatrix();
        DiceCount = 25;
        StartCoroutine(SpawnDice(DiceCount));
        OnScoreChanged?.Invoke();
        OnDiceCountChange?.Invoke();
    }

    public void OnTossButton()
    {
        if (!IsAllSleeping())
        {
            return;
        }
        MovingDicesCount = AllDice.Count;
        score = 0;
        OnScoreChanged?.Invoke();
        foreach (OneDiceController dice in AllDice)
        {
            dice.Toss();
        }
    }

    public bool IsAllSleeping()
    {
        return allDice.All(dice => dice.DiceRigidbody.IsSleeping());
    }

    private void SetScore()
    {
        foreach (var dice in AllDice)
        {
            dice.SetRolledValue();
            score += dice.RolledValue;
        }
    }

    private void ReduceMovingDicesCount()
    {
        MovingDicesCount -= 1;
    }

    public void SetDiceCount(int neededCount)
    {
        DiceCount = neededCount;
        OnDiceCountChange?.Invoke();
        var difference = DiceCount - AllDice.Count;
        maxScore = DiceCount * 6;
        if (difference < 0)
        {
            DestroyDice(-difference);
        }
        else if (difference > 0)
        {
            StartCoroutine(SpawnDice(difference));
        }
    }

    public void SetWinPoints()
    {

    }

    private void DestroyDice(int timesCount)
    {
        while (timesCount != 0)
        {

            var lastIndex = allDice.Count - 1;
            var dice = allDice[lastIndex];
            dice.OnDiceLand.RemoveListener(ReduceMovingDicesCount);
            Destroy(dice.gameObject);
            allDice.RemoveAt(lastIndex);
            timesCount--;
        }
    }

    private IEnumerator SpawnDice(int diceToSpawnCount)
    {
        isSpawnCoroutineStarted = true;
        while (diceToSpawnCount != 0)
        {
            yield return new WaitForSeconds(0.1f); //ждем пока предыдущий слой опустится
            foreach (var pos in spawnPoints)
            {
                var dice = Instantiate(prefab, transform);
                dice.SetAllDiceController(this);
                dice.OnDiceLand.AddListener(ReduceMovingDicesCount);
                dice.transform.position = pos;
                allDice.Add(dice);
                diceToSpawnCount--;
                if (diceToSpawnCount == 0)
                {
                    break;
                }
            }
        }
        yield return new WaitForSeconds(2f); //ждем пока последний слой долетит
        isSpawnCoroutineStarted = false;
    }

    private Vector3[,] GetSpawnMatrix()
    {
        var rowCount = 5;
        var spawnPoints = new Vector3[rowCount, rowCount];
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;
        var spaceBeetweenPoints = 2f;
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < rowCount; j++)
            {
                spawnPoints[i, j] = new Vector3(x + spaceBeetweenPoints * i, y, z + spaceBeetweenPoints * j);
            }
        }
        return spawnPoints;
    }

    private void OnDisable()
    {
        foreach (var dice in AllDice)
        {
            dice.OnDiceLand.RemoveListener(ReduceMovingDicesCount);
        }
    }
}