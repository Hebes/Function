using TMPro;
using UnityEngine;

namespace ExpansionUnity
{
    public static  class ExpansionTextMeshPro
    {
        public static TextMeshProUGUI GetTextMeshPro(this GameObject gameObject)
        {
            return gameObject.GetComponent<TextMeshProUGUI>();
        }
        
        public static TextMeshProUGUI GetTextMeshPro(this Transform transform)
        {
            return transform.GetComponent<TextMeshProUGUI>();
        }
    }
}
