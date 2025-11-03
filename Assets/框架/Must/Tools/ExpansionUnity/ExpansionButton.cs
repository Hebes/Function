using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExpansionUnity
{
    public static class ExpansionButton
    {
        public static Button GetButton(this GameObject gameObject)
        {
            return gameObject.GetComponent<Button>();
        }

        public static Button GetButton(this Transform transform)
        {
            return transform.GetComponent<Button>();
        }

        public static Button AddButton(this Transform transform, UnityAction call)
        {
            return AddButton(transform.gameObject, call);
        }

        public static Button AddButton(this GameObject gameObject, UnityAction call)
        {
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(call);
            return button;
        }
    }
}