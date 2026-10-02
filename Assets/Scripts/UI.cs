using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    public void UpdateText(int score)
    {
        textLabel.text = $"Количество очков: {score.ToString()}";
    }
}
