using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 摇杆类型
/// </summary>
public enum E_JoystickType
{
    /// <summary>
    /// 固定摇杆
    /// </summary>
    Normal,

    /// <summary>
    /// 可变位置摇杆
    /// </summary>
    CanChangePos,

    /// <summary>
    /// 可移动摇杆
    /// </summary>
    CanMove,
}


public class JpyStickPanel : MonoBehaviour
{
    public static JpyStickPanel Instance { get; private set; }
    [Header("遥感类型")] public E_JoystickType joystickType = E_JoystickType.Normal;
    [Header("159代表ImageBK的Wight一半")] public float maxL = 150;
    [Header("控制遥感的范围")] public Image imageTouchRect;
    [Header("摇杆背景图片")] public Image imgBk;
    [Header("摇杆圆圈")] public Image imgControl;

    #region 事件触发
    public Action<Vector2> move;
    #endregion
    private Vector2 temp;

    private void Awake()
    {
        Instance = this;

        imageTouchRect = transform.Find("ImgTouchRect").GetComponent<Image>();
        imgBk = transform.Find("ImgTouchRect/ImageBK").GetComponent<Image>();
        imgControl = transform.Find("ImgTouchRect/ImageBK/ImgControl").GetComponent<Image>();

        AddCustomEventListener(imageTouchRect, EventTriggerType.PointerDown, PointerDown);
        AddCustomEventListener(imageTouchRect, EventTriggerType.PointerUp, PointerUp);
        AddCustomEventListener(imageTouchRect, EventTriggerType.Drag, Drag);

        switch (joystickType)
        {
            default:
            case E_JoystickType.Normal:
                imgBk.gameObject.SetActive(true);
                break;
            case E_JoystickType.CanChangePos:
            case E_JoystickType.CanMove:
                imgBk.gameObject.SetActive(false);
                break;
        }
    }

    private void PointerDown(BaseEventData data)
    {
        if (data is not PointerEventData pointerEventData) return;
        Debug.Log("Down");

        //可变位置摇杆 - 按下显示
        imgBk.gameObject.SetActive(true);

        switch (joystickType)
        {
            default:
            case E_JoystickType.Normal: break;
            case E_JoystickType.CanChangePos:
            case E_JoystickType.CanMove:
                //可变位置摇杆 - 点击屏幕位置显示 
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    imageTouchRect.rectTransform, //你想要改变位置的对象的父对象
                    pointerEventData.position, //得到当前屏幕鼠标位置
                    pointerEventData.pressEventCamera, // UI用的摄像机
                    out Vector2 localPos); //可以得到一个转换来的相对坐标

                imgBk.transform.localPosition = localPos;
                break; //可变位置摇杆 - 开始隐藏
        }
    }


    private void PointerUp(BaseEventData data)
    {
        Debug.Log("Up");
        imgControl.transform.localPosition = Vector2.zero;
        move?.Invoke(Vector2.zero);
        //temp = Vector2.zero;
        switch (joystickType)
        {
            default:
            case E_JoystickType.Normal:
                imgBk.gameObject.SetActive(true);
                break;
            case E_JoystickType.CanChangePos:
            case E_JoystickType.CanMove:
                imgBk.gameObject.SetActive(false);
                break; //可变位置摇杆 - 开始隐藏
        }
    }

    private void Drag(BaseEventData data)
    {
        if (data is not PointerEventData pointerEventData) return;
        Debug.Log("Drag");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBk.rectTransform, //你想要改变位置的对象的父对象
            pointerEventData.position, //得到当前屏幕鼠标位置
            pointerEventData.pressEventCamera, // UI用的摄像机
            out Vector2 localPos); //可以得到一个转换来的相对坐标

        //更新位置
        imgControl.transform.localPosition = localPos;


        //范围判断
        if (localPos.magnitude > maxL) //159代表ImageBK的Wight一半
        {
            switch (joystickType)
            {
                default:
                case E_JoystickType.Normal:
                case E_JoystickType.CanChangePos: break;
                case E_JoystickType.CanMove:
                    imgBk.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - maxL));
                    break; //超出多少就让背景图动多少
            }

            //超出范围 等于这个范围
            imgControl.transform.localPosition = localPos.normalized * maxL;
        }

        move?.Invoke(localPos.normalized);
        //temp = localPos.normalized;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    private void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>() ?? control.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}