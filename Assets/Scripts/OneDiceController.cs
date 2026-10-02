using UnityEngine;

public class OneDiceController : MonoBehaviour
{
    [SerializeField] private Rigidbody diceRigidbody;
    [SerializeField] AllDiceController allDiceController;

    private int rolledValue;

    public AllDiceController AllDiceController => allDiceController;
    public int RolledValue => rolledValue;
    public Rigidbody DiceRigidbody => diceRigidbody;

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

    public bool IsMoving()
    {
        var velocity = DiceRigidbody.linearVelocity;
        return velocity.x > 0.01f || velocity.y > 0.01f || velocity.z > 0.01f ? true : false;
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
}
