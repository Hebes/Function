using UnityEngine;
using UnityEngine.UI;

namespace ExpansionUnity
{
    public static class ExpansionSlider
    {
        public static Slider GetSlider(this GameObject gameObject)
        {
            return gameObject.GetComponent<Slider>();
        }

        public static Slider GetSlider(this Transform transform)
        {
            return transform.GetComponent<Slider>();
        }
    }
}