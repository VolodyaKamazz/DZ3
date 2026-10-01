using Unity.VisualScripting;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody diceRigidbody;

    [Range(1, 6)] private int rolledValue;

    public int RolledValue => rolledValue;
    public Rigidbody Rigidbody => diceRigidbody;

    private void Awake()
    {
        if (Rigidbody == null)
        {
            throw new System.Exception("NullRigidbodyReferenceException");
        }
    }
}
