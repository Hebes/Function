using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                string message = "";
                for (int i = 0; i < Random.Range(1, 32); i++)
                {
                    message += "F";
                }

                MessageManager1.Instance.Add(message);
            }
        }
    }
}