using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bindedButtonText;
    [SerializeField] private Button control;
    [SerializeField] private BindsController bindsController;
    [SerializeField] private AllDiceController allDiceController;
    
    private void Awake()
    {
        bindedButtonText.text = $"Бросок: {bindsController.Action.GetBindingDisplayString(0)}";
    }

    public void UpdateScoreText()
    {
        if (allDiceController.Score == 0) 
        {
            scoreText.text = "Количество очков:";
            return;
        }
        scoreText.text = $"Количество очков: {allDiceController.Score}";
    }

    private void OnEnable()
    {
        bindsController.OnButtonChange.AddListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.AddListener(UpdateScoreText);
    }

    private void OnDisable()
    {
        bindsController.OnButtonChange.RemoveListener(UpdateBindedButtonText);
        allDiceController.OnScoreChanged.RemoveListener(UpdateScoreText);
    }

    public void UpdateBindedButtonText(string buttonName)
    {
        bindedButtonText.text = $"Бросок: {buttonName}";
    }
}