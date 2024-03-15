using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    public RectTransform rectTransformPanel;

    public RectTransform rectTransformText;

    public Text textMessage;

    [HideInInspector] public float timePositionX;

    void Update()
    {
        rectTransformPanel.localPosition = new Vector3(
            MessageManager1.Instance.curvePositionX.Evaluate(timePositionX),
            rectTransformPanel.localPosition.y
        );
        timePositionX += Time.deltaTime;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator ColorAlphaChange()
    {
        float startTime = Time.time;
        float timeInterval;
        while (true)
        {
            yield return 0;
            timeInterval = Time.time - startTime;
            if (timeInterval < .1f)
            {
                CanvasGroup.alpha = MessageManager1.Instance.curveColorAlpha.Evaluate(timeInterval);
            }
            else
            {
                CanvasGroup.alpha = MessageManager1.Instance.curveColorAlpha.Evaluate(.2f);
                yield break;
            }
        }
    }
}