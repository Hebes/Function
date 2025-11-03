using UnityEngine;
using UnityEngine.UI;

namespace ExpansionUnity
{
    public static class ExpansionImage
    {
        public static Image GetImage(this GameObject gameObject)
        {
            return gameObject.GetComponent<Image>();
        }

        public static Image GetImage(this Transform transform)
        {
            return transform.GetComponent<Image>();
        }

        public static Image GetImageChild(this Transform transform, string path)
        {
            return transform.GetChild(path).GetImage();
        }
    }
}