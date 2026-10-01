using System.Collections;
using UnityEngine;

namespace Assets
{
    public class FaceController : MonoBehaviour
    {
        [SerializeField][Range(1, 6)] private int faceValue;

        public int FaceValue => faceValue;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Table"))
            {
                return;
            }
            Debug.Log($"{transform.parent.name} коснулся поверхности гранью номер {FaceValue}");
        }
    }
}