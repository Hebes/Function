using UnityEngine;
using UnityEngine.UI;

namespace ExpansionUnity
{
    public static class ExpansionGameObject
    {
        public static void SetActive(this Image image, bool value)
        {
            image.gameObject.SetActive(value);
        }

        public static void SetActive(this Transform transform, bool value)
        {
            transform.gameObject.SetActive(value);
        }

        public static GameObject Instantiate(this GameObject gameObject)
        {
            return Object.Instantiate(gameObject);
        }
    }
}