using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private Button control;

    public void UpdateText(int score)
    {
        textLabel.text = $"Количество очков: {score.ToString()}";
    }
}
