using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AllDiceController : MonoBehaviour
{
    [SerializeField] private int maxDiceCount;
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
    private GameOutcome gameResult;
    private bool isWinPointsRandom = true;
    private bool isDrawPointsRandom = true;
    private int maxScore;
    private Vector3[,] spawnPoints;
    private bool isSpawnCoroutineStarted;

    public List<OneDiceController> AllDice => allDice;
    public float Torque => torque;
    public int Score => score;
    public int MinForce => minForce;
    public int MaxForce => maxForce;
    public GameOutcome GameResult => gameResult;
    public bool IsWinPointsRandom => isWinPointsRandom;
    public bool IsDrawPointsRandom => isDrawPointsRandom;
    public bool IsTossing => movingDicesCount > 0;
    private bool IsProcessed => IsTossing || isSpawnCoroutineStarted;
    public int WinPoints
    {
        get => winPoints;
        private set
        {
            winPoints = Mathf.Clamp(value, DiceCount + 2, maxScore);
            OnWinPointsChanged?.Invoke();
        }
    }
    public int DrawPoints
    {
        get => drawPoints;
        private set
        {
            drawPoints = Mathf.Clamp(value, DiceCount + 1, winPoints - 1);
            OnDrawPointsChanged?.Invoke();
        }
    }
    public int DiceCount
    {
        get => diceCount;
        private set
        {
            diceCount = Mathf.Clamp(value, 1, maxDiceCount);
            OnDiceCountChanged?.Invoke();
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
                SetGameResult();
                OnScoreChanged?.Invoke();
            }
        }
    }

    public UnityEvent OnScoreChanged;
    public UnityEvent OnDiceCountChanged;
    public UnityEvent OnWinPointsChanged;
    public UnityEvent OnDrawPointsChanged;
    public UnityEvent<bool> OnWinPointsIsRandomChanged;
    public UnityEvent<bool> OnDrawPointsIsRandomChanged;
    public UnityEvent OnGameResultChanged;

    private void Awake()
    {
        spawnPoints = GetSpawnMatrix();
        SetDiceCount(25);
    }

    public void OnTossButton()
    {
        if (IsProcessed || !IsAllSleeping())
        {
            return;
        }
        UpdatePoints(true);
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
        if (!IsTossing)
        {
            return;
        }
        MovingDicesCount -= 1;
    }

    public void SetDiceCount(int inputDiceCount)
    {
        if (IsProcessed)
        {
            OnDiceCountChanged?.Invoke();
            return;
        }
        DiceCount = inputDiceCount;
        var difference = DiceCount - AllDice.Count;
        if (difference == 0)
        {
            return;
        }
        maxScore = DiceCount * 6;
        if (difference < 0)
        {
            DestroyDice(-difference);
        }
        else
        {
            StartCoroutine(SpawnDice(difference));
        }
        score = 0;
        UpdatePoints(true);
        OnScoreChanged?.Invoke();
    }

    public void SetWinPoints(int inputWinPoints)
    {
        if (IsTossing || isWinPointsRandom)
        {
            OnWinPointsChanged?.Invoke();
            return;
        }
        winPoints = inputWinPoints;
        UpdatePoints(false);
    }

    public void SetDrawPoints(int inputDrawPoints)
    {
        if (IsTossing || isDrawPointsRandom)
        {
            OnDrawPointsChanged?.Invoke();
            return;
        }
        drawPoints = inputDrawPoints;
        UpdatePoints(false);
    }

    public void SetWinPointsIsRandom(bool isRandom)
    {
        if (!IsTossing)
        {
            isWinPointsRandom = isRandom;
        }
        OnWinPointsIsRandomChanged?.Invoke(isWinPointsRandom);
    }

    public void SetDrawPointsIsRandom(bool isRandom)
    {
        if (!IsTossing)
        {
            isDrawPointsRandom = isRandom;
        }
        OnDrawPointsIsRandomChanged?.Invoke(isDrawPointsRandom);
    }

    private void UpdatePoints(bool reroll)
    {
        if (reroll && isWinPointsRandom)
        {
            WinPoints = UnityEngine.Random.Range(DiceCount + 2, maxScore + 1);
        }
        else
        {
            WinPoints = winPoints;
        }
        if (reroll && isDrawPointsRandom)
        {
            DrawPoints = UnityEngine.Random.Range(DiceCount + 1, winPoints);
        }
        else
        {
            DrawPoints = drawPoints;
        }
    }

    private void SetGameResult()
    {
        if (score >= winPoints)
        {
            gameResult = GameOutcome.Win;
        }
        else if (score >= drawPoints)
        {
            gameResult = GameOutcome.Draw;
        }
        else
        {
            gameResult = GameOutcome.Lose;
        }
        OnGameResultChanged?.Invoke();
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