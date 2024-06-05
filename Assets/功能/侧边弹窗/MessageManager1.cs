using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 侧边显示消息(可自定义位置)
/// 
/// https://developer.unity.cn/projects/5e327289edbc2a3491cb49b5
/// 1.使用AnimationCurve类画几段平滑的曲线，来使消息的滑入滑出有轻微的加速减速过程，尽可能地提升还原度和观感；
/// 2.一次性收到过多消息，并且同时显示出来，会导致有些消息来不及看，所以我们要做一个消息缓冲区，以及一个弹出的“冷却时间”；
/// 3.使用TextMeshPro所提供的TextMeshProUGUI.preferredWidth来获取特定字符串在UI上占据的实际宽度，以实现背景黑方块宽度对文字宽度的匹配；
/// 4.使用CanvasGroup.alpha来调整消息UI的整体透明度。
/// </summary>
public class MessageManager1 : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static MessageManager1 Instance;

    /// <summary>
    /// 
    /// </summary>
    public AnimationCurve curveColorAlpha;

    public AnimationCurve curvePositionX;

    public Transform mask;

    public Transform pivot;

    public Object prefabPanelMessage;

    private bool _mIsMovingUp;

    private List<string> _mMessageCacheList;

    private List<Message> _mQueueMessages;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _mIsMovingUp = false;
        _mMessageCacheList = new List<string>();
        _mQueueMessages = new List<Message>();
    }

    private void Update()
    {
        if (!_mIsMovingUp && _mMessageCacheList.Count > 0)
        {
            _mIsMovingUp = true;
            Show();
        }

        QueueClear();
    }

    public void Add(string message)
    {
        _mMessageCacheList.Add(message);
    }

    private IEnumerator MoveUp(float height)
    {
        float startPositionY = pivot.localPosition.y;
        float startTime = Time.time;
        float timeInterval;
        while (true)
        {
            yield return 0;
            timeInterval = Time.time - startTime;
            if (timeInterval < .1f)
            {
                pivot.localPosition = new Vector3(pivot.localPosition.x,
                    height * timeInterval * 10f + startPositionY);
            }
            else
            {
                pivot.localPosition = new Vector3(pivot.localPosition.x,
                    height + startPositionY);
                _mIsMovingUp = false;
                yield break;
            }
        }
    }

    private void QueueClear()
    {
        for (int a = 0; a < _mQueueMessages.Count; ++a)
        {
            if (_mQueueMessages[0].timePositionX > 5f)
            {
                GameObject gameObject = _mQueueMessages[0].gameObject;
                _mQueueMessages.RemoveAt(0);
                Destroy(gameObject);
            }
            else
            {
                break;
            }
        }
    }

    private void Show()
    {
        Message message = (Instantiate(prefabPanelMessage, mask) as GameObject).GetComponent<Message>();
        message.transform.SetParent(pivot);

        if (_mQueueMessages.Count > 0)
        {
            StartCoroutine(_mQueueMessages[_mQueueMessages.Count - 1].ColorAlphaChange());
        }

        _mQueueMessages.Add(message);
        message.textMessage.text = _mMessageCacheList[0];
        _mMessageCacheList.RemoveAt(0);
        float preferredHeight = message.textMessage.preferredHeight;
        float preferredWidth = message.textMessage.preferredWidth;
        message.rectTransformPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight + 12f);
        message.rectTransformText.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);

        if (preferredWidth < 460f)
        {
            message.rectTransformPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth + 20f);
        }
        else
        {
            message.rectTransformPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
        }

        message.timePositionX = 0f;
        StartCoroutine(MoveUp(preferredHeight + 32f));
    }
}