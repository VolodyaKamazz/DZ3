using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button bindingButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private BindsController bindsController;
    [SerializeField] private AllDiceController allDiceController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bindedButtonText;
    [SerializeField] private TMP_Text losePointsText;
    [SerializeField] private TMP_Text gameResultText;
    
    [Header("Dice Count")]
    [SerializeField] private TMP_Text diceCountText;
    [SerializeField] private TMP_InputField diceCountField;

    [Header("Win Points")]
    [SerializeField] private TMP_Text winPointsText;
    [SerializeField] private TMP_InputField winPointsField;
    [SerializeField] private Toggle winToggle;

    [Header("Draw Points")]
    [SerializeField] private TMP_Text drawPointsText;
    [SerializeField] private TMP_InputField drawPointsField;
    [SerializeField] private Toggle drawToggle;

    private void Awake()
    {
        UpdateBindedButtonText(bindsController.Action.GetBindingDisplayString(0));
        UpdateScoreText();
        UpdateDiceCountText();
        UpdateWinPointsText();
        UpdateDrawPointsText();
        SetWinPointsFieldActivity(allDiceController.IsWinPointsRandom);
        SetDrawPointsFieldActivity(allDiceController.IsDrawPointsRandom);
    }

    private void UpdateScoreText()
    {
        UpdateFieldsInteractable();
        if (allDiceController.Score == 0)
        {
            scoreText.text = "Количество очков: -";
            gameResultText.text = "Результат: -";
            return;
        }
        scoreText.text = $"Количество очков: {allDiceController.Score}";
    }

    private void UpdateBindedButtonText(string buttonName)
    {
        bindedButtonText.text = $"Бросок: {buttonName}";
    }

    private void UpdateDiceCountText()
    {
        diceCountText.text = $"Число\nкубиков: {allDiceController.DiceCount}";
        diceCountField.SetTextWithoutNotify(allDiceController.DiceCount.ToString());
    }

    private void UpdateWinPointsText()
    {
        winPointsText.text = $"Победа: >= {allDiceController.WinPoints}";
        winPointsField.SetTextWithoutNotify(allDiceController.WinPoints.ToString());
    }

    private void UpdateDrawPointsText()
    {
        drawPointsText.text = $"Ничья: >= {allDiceController.DrawPoints}";
        losePointsText.text = $"Пораж.: < {allDiceController.DrawPoints}";
        drawPointsField.SetTextWithoutNotify(allDiceController.DrawPoints.ToString());
    }

    private void UpdateGameResultText()
    {
        gameResultText.text = allDiceController.GameResult switch
        {
            GameOutcome.Win => "Результат: Победа",
            GameOutcome.Draw => "Результат: Ничья",
            GameOutcome.Lose => "Результат: Поражение",
            _ => "Результат: -"
        };
    }

    private void SetDiceCount(string inputDiceCount)
    {
        if (int.TryParse(inputDiceCount, out int value))
        {
            allDiceController.SetDiceCount(value);
            return;
        }
        UpdateDiceCountText();
    }

    private void SetWinPoints(string inputWinPoints)
    {
        if (int.TryParse(inputWinPoints, out int value))
        {
            allDiceController.SetWinPoints(value);
            return;
        }
        UpdateWinPointsText();
    }

    private void SetDrawPoints(string inputDrawPoints)
    {
        if (int.TryParse(inputDrawPoints, out int value))
        {
            allDiceController.SetDrawPoints(value);
            return;
        }
        UpdateDrawPointsText();
    }

    private void SetWinPointsFieldActivity(bool isRandom)
    {
        winToggle.SetIsOnWithoutNotify(isRandom);
        UpdateFieldsInteractable();
    }

    private void SetDrawPointsFieldActivity(bool isRandom)
    {
        drawToggle.SetIsOnWithoutNotify(isRandom);
        UpdateFieldsInteractable();
    }

    private void UpdateFieldsInteractable()
    {
        var canEdit = !allDiceController.IsTossing;
        diceCountField.interactable = canEdit;
        winToggle.interactable = canEdit;
        drawToggle.interactable = canEdit;
        winPointsField.interactable = canEdit && !winToggle.isOn;
        drawPointsField.interactable = canEdit && !drawToggle.isOn;
    }

    private void OnEnable()
    {
        bindingButton.onClick.AddListener(bindsController.RebindButton);
        rollButton.onClick.AddListener(allDiceController.OnTossButton);
        bindsController.OnButtonChange.AddListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.AddListener(UpdateScoreText);
        allDiceController.OnDiceCountChanged.AddListener(UpdateDiceCountText);
        allDiceController.OnWinPointsChanged.AddListener(UpdateWinPointsText);
        allDiceController.OnDrawPointsChanged.AddListener(UpdateDrawPointsText);
        allDiceController.OnWinPointsIsRandomChanged.AddListener(SetWinPointsFieldActivity);
        allDiceController.OnDrawPointsIsRandomChanged.AddListener(SetDrawPointsFieldActivity);
        allDiceController.OnGameResultChanged.AddListener(UpdateGameResultText);
        diceCountField.onEndEdit.AddListener(SetDiceCount);
        winPointsField.onEndEdit.AddListener(SetWinPoints);
        drawPointsField.onEndEdit.AddListener(SetDrawPoints);
        winToggle.onValueChanged.AddListener(allDiceController.SetWinPointsIsRandom);
        drawToggle.onValueChanged.AddListener(allDiceController.SetDrawPointsIsRandom);
    }

    private void OnDisable()
    {
        bindingButton.onClick.RemoveListener(bindsController.RebindButton);
        rollButton.onClick.RemoveListener(allDiceController.OnTossButton);
        bindsController.OnButtonChange.RemoveListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.RemoveListener(UpdateScoreText);
        allDiceController.OnDiceCountChanged.RemoveListener(UpdateDiceCountText);
        allDiceController.OnWinPointsChanged.RemoveListener(UpdateWinPointsText);
        allDiceController.OnDrawPointsChanged.RemoveListener(UpdateDrawPointsText);
        allDiceController.OnWinPointsIsRandomChanged.RemoveListener(SetWinPointsFieldActivity);
        allDiceController.OnDrawPointsIsRandomChanged.RemoveListener(SetDrawPointsFieldActivity);
        allDiceController.OnGameResultChanged.RemoveListener(UpdateGameResultText);
        diceCountField.onEndEdit.RemoveListener(SetDiceCount);
        winPointsField.onEndEdit.RemoveListener(SetWinPoints);
        drawPointsField.onEndEdit.RemoveListener(SetDrawPoints);
        winToggle.onValueChanged.RemoveListener(allDiceController.SetWinPointsIsRandom);
        drawToggle.onValueChanged.RemoveListener(allDiceController.SetDrawPointsIsRandom);
    }
}