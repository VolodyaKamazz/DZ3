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
    
    private void Awake()
    {
        bindedButtonText.text = $"Бросок: {bindsController.Action.GetBindingDisplayString(0)}";
    }

    public void UpdateText(int score)
    {
        scoreText.text = $"Количество очков: {score.ToString()}";
    }

    public void UpdateBindedButtonText(string buttonName)
    {
        bindedButtonText.text = $"Бросок: {buttonName}";
    }
}