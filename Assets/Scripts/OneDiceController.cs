using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OneDiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody diceRigidbody;
    [SerializeField] private AllDiceController allDiceController;

    private int rolledValue;
    private bool isCoroutineStarted;

    public AllDiceController AllDiceController => allDiceController;
    public int RolledValue => rolledValue;
    public Rigidbody DiceRigidbody => diceRigidbody;

    public UnityEvent OnDiceLand;

    public void SetAllDiceController(AllDiceController controller)
    {
        allDiceController = controller;
    }

    public void Toss()
    {
        var force = new Vector3(UnityEngine.Random.Range(-AllDiceController.MaxForce, AllDiceController.MaxForce),
                                UnityEngine.Random.Range(AllDiceController.MinForce, AllDiceController.MaxForce),
                                UnityEngine.Random.Range(-AllDiceController.MaxForce, AllDiceController.MaxForce));
        var position = new Vector3(UnityEngine.Random.Range(-AllDiceController.Torque, AllDiceController.Torque),
                                   UnityEngine.Random.Range(-AllDiceController.Torque, AllDiceController.Torque),
                                   UnityEngine.Random.Range(-AllDiceController.Torque, AllDiceController.Torque));
        DiceRigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
    }

    public void SetRolledValue()
    {
        var maxY = -1f;
        var rolledFace = "";
        foreach (Transform child in transform)
        {
            if (child.position.y > maxY)
            {
                maxY = child.position.y;
                rolledFace = child.name;
            }
        }
        rolledValue = rolledFace switch
        {
            "Face 1" => 1,
            "Face 2" => 2,
            "Face 3" => 3,
            "Face 4" => 4,
            "Face 5" => 5,
            "Face 6" => 6,
            _ => 0
        };
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (isCoroutineStarted || !(collision.transform.tag != "Table" || collision.transform.tag != "Dice"))
        {
            return;
        }
        StartCoroutine(WaitUntilDiceStopRolling());
    }

    private IEnumerator WaitUntilDiceStopRolling()
    {
        isCoroutineStarted = true;
        while (!DiceRigidbody.IsSleeping())
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnDiceLand?.Invoke();
        isCoroutineStarted = false;
    }
}