using UnityEngine;

namespace Assets
{
    public class FaceController : MonoBehaviour
    {
        [SerializeField] private DiceFace faceValue;
        [SerializeField] private DiceController parentDice;

        public DiceFace FaceValue => faceValue;
        public DiceController ParentDice => parentDice;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Table"))
            {
                Debug.Log($"{ParentDice.name} коснулся поверхности гранью номер {FaceValue}");
                ParentDice.SetRolledValue(FaceValue);
            }
        }
    }
}