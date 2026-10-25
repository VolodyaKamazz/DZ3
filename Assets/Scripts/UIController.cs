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

    [Header("Dice Count")]
    [SerializeField] private TMP_Text diceCountText;
    [SerializeField] private TMP_InputField diceCountField;

    [Header("Win Points")]
    [SerializeField] private TMP_Text winPointsText;
    [SerializeField] private TMP_InputField winPointsField;

    [Header("Draw Points")]
    [SerializeField] private TMP_Text drawPointsText;
    [SerializeField] private TMP_InputField drawPointsField;

    [Header("Lose Points")]
    [SerializeField] private TMP_Text losePointsText;
    [SerializeField] private TMP_InputField losePointsField;

    private void Awake()
    {
        bindedButtonText.text = $"Бросок: {bindsController.Action.GetBindingDisplayString(0)}";
    }

    public void UpdateScoreText()
    {
        if (allDiceController.Score == 0) 
        {
            scoreText.text = "Количество очков: -";
            return;
        }
        scoreText.text = $"Количество очков: {allDiceController.Score}";
    }

    public void UpdateBindedButtonText(string buttonName)
    {
        bindedButtonText.text = $"Бросок: {buttonName}";
    }

    public void UpdateDiceCountText()
    {
        diceCountText.text = $"Число\nкубиков: {allDiceController.DiceCount}";
    }

    private void SetDiceCount(string diceCount)
    {
        if (int.TryParse(diceCount, out int value))
        {
            allDiceController.SetDiceCount(value);
            diceCountField.text = allDiceController.DiceCount.ToString();
        }
    }

    private void OnEnable()
    {
        bindsController.OnButtonChange.AddListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.AddListener(UpdateScoreText);
        allDiceController.OnDiceCountChange.AddListener(UpdateDiceCountText);
        bindingButton.onClick.AddListener(bindsController.RebindButton);
        rollButton.onClick.AddListener(allDiceController.OnTossButton);
        diceCountField.onEndEdit.AddListener(SetDiceCount);
    }

    private void OnDisable()
    {
        bindsController.OnButtonChange.RemoveListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.RemoveListener(UpdateScoreText);
        allDiceController.OnDiceCountChange.RemoveListener(UpdateDiceCountText);
    }
}