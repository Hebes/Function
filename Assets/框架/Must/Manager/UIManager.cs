using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EUIType
{
    /// <summary> 普通窗体 </summary>
    Normal,

    /// <summary> 固定窗体 </summary>
    Fixed,

    /// <summary> 弹出窗体 </summary>
    PopUp,

    /// <summary> 独立的窗口可移动的 </summary>
    Mobile,

    /// <summary> 渐变过度窗体 </summary>
    Fade,

    /// <summary> 主要的,存放主菜单UI </summary>
    Main,
}

public enum EUIMode
{
    /// <summary> 普通 模式允许多个窗体同时显示 </summary>
    Normal,

    /// <summary> 反向切换 一般要求玩家必须先关闭弹出的顶层窗体，再依次关闭下一级窗体</summary>
    ReverseChange,

    /// <summary> 隐藏其他 一般应用于全局性的窗体 </summary>
    HideOther,
}

public class UIManager : MonoBehaviour
{
    // private static UIManager _i;
    // public static UIManager I => _i ??= new UIManager();
    public GameObject canvasRoot = null; //UI根节点    
    private readonly Dictionary<string, UIBase> _uiBuffer = new(); //UI缓存
    private readonly Dictionary<string, UIBase> _currentShowUI = new(); //当前显示的UI
    private readonly Stack<UIBase> _staCurrentUI = new(); //定义“栈”集合,存储显示当前所有[反向切换]的窗体类型
    private string _uiPath;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        //根
        canvasRoot = new GameObject("CanvasRoot");
        Canvas canvas = canvasRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScale = canvasRoot.AddComponent<CanvasScaler>();
        canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScale.referenceResolution = new Vector2(1920, 1080);
        canvasRoot.AddComponent<GraphicRaycaster>();

        //子节点
        CreatChild(canvasRoot.transform, "Normal");
        CreatChild(canvasRoot.transform, "Fixed");
        CreatChild(canvasRoot.transform, "Mobile");
        CreatChild(canvasRoot.transform, "PopUp");
        CreatChild(canvasRoot.transform, "Fade");
        CreatChild(canvasRoot.transform, "Main");
    }

    private void CreatChild(Transform parent, string goName)
    {
        GameObject def = new GameObject(goName);
        def.transform.SetParent(parent);
        def.AddComponent<CanvasRenderer>();
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public void ShowPanel<T>() where T : UIBase
    {
        string uiName = typeof(T).FullName;
        if (string.IsNullOrEmpty(uiName)) return;
        T t = null;
        if (_uiBuffer.ContainsKey(uiName))
            t = _uiBuffer[uiName] as T;
        else
            t = LoadPanel<T>();

        UIBase baseUi = null;
        switch (t.mode)
        {
            case EUIMode.Normal:
                if (_currentShowUI.ContainsKey(uiName)) break;
                if (_uiBuffer.TryGetValue(uiName, out baseUi))
                {
                    _currentShowUI.Add(uiName, baseUi);
                    baseUi.OnEnable();
                }

                break;
            case EUIMode.ReverseChange:
                if (_staCurrentUI.Count > 0)//判断“栈”集合中，是否有其他的窗体，有则“冻结”处理。
                {
                    UIBase topUIForm = _staCurrentUI.Peek();
                    //topUIForm.Freeze(); //栈顶元素作冻结处理 冻结状态（即：窗体显示在其他窗体下面）TODO 需要重新思考
                }

                if (_uiBuffer.TryGetValue(uiName, out baseUi))//判断“UI所有窗体”集合是否有指定的UI窗体，有则处理。
                {
                    baseUi.OnEnable();//当前窗口显示状态
                    _staCurrentUI.Push(baseUi);//把指定的UI窗体，入栈操作。
                }
                else
                {
                    Debug.LogError($"{uiName}是空的");
                }
                break;
            case EUIMode.HideOther:
                if (_currentShowUI.TryGetValue(uiName, out baseUi))break;
                //把“正在显示集合”与“栈集合”中所有窗体都隐藏。
                foreach (UIBase baseUI in _currentShowUI.Values)
                    baseUI.OnDisable();
                foreach (UIBase staUI in _staCurrentUI)
                    staUI.OnDisable();

                //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
                if (_uiBuffer.TryGetValue(uiName, out UIBase baseUIFormFromALL))
                {
                    _currentShowUI.Add(uiName, baseUIFormFromALL);
                    baseUIFormFromALL.OnEnable();//窗体显示
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private T LoadPanel<T>() where T : UIBase
    {
        string uiName = typeof(T).FullName;
        T handle = Resources.Load<T>($"{_uiPath}/{uiName}");
        Transform chileNode = canvasRoot.transform.Find(handle.type.ToString());
        T t = GameObject.Instantiate(handle, chileNode, false);
        if (t == null) throw new Exception("加载预制体失败");
        t.OnCreate();
        _uiBuffer.Add(uiName, t);
        return t;
    }

    public T GetUIPanel<T>() where T : UIBase
    {
        string uiName = typeof(T).FullName;
        if (_uiBuffer.TryGetValue(uiName, out UIBase baseUiForm))
            return baseUiForm as T;
        return default;
    }

    public void CloseUI<T>()
    {
        string uiName = typeof(T).FullName;
        if (!_uiBuffer.ContainsKey(uiName)) return;

        switch (_uiBuffer[uiName].mode)
        {
            case EUIMode.Normal:
                if (_currentShowUI.ContainsKey(uiName))
                {
                    UIBase uiBase = _currentShowUI[uiName];
                    uiBase.OnDisable();
                    uiBase.gameObject.SetActive(false);
                    _currentShowUI.Remove(uiName);
                }

                break;
            case EUIMode.ReverseChange:
                if (_staCurrentUI.Count >= 2)
                {
                    UIBase topUIForms = _staCurrentUI.Pop(); //出栈处理
                    topUIForms.OnDisable(); //做隐藏处理
                    UIBase nextUIForms = _staCurrentUI.Peek(); //出栈后，下一个窗体做“重新显示”处理。
                    nextUIForms.OnDisable();
                }
                else if (_staCurrentUI.Count == 1)
                {
                    UIBase topUIForms = _staCurrentUI.Pop(); //出栈处理
                    topUIForms.OnDisable(); //做隐藏处理
                }

                break;
            case EUIMode.HideOther:
                //当前窗体隐藏状态，且“正在显示”集合中，移除本窗体
                if (_currentShowUI.ContainsKey(uiName))
                {
                    UIBase uiBase = _currentShowUI[uiName];
                    uiBase.OnDisable();
                    uiBase.gameObject.SetActive(false);
                    _currentShowUI.Remove(uiName);
                }

                //把“正在显示集合”与“栈集合”中所有窗体都定义重新显示状态。
                foreach (UIBase baseUI in _currentShowUI.Values)
                    baseUI.OnEnable();
                foreach (UIBase staUI in _staCurrentUI)
                    staUI.OnEnable();
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public void RemoveUI<T>(string uiFormName) where T : UIBase
    {
        string uiName = typeof(T).FullName;
        if (_currentShowUI.TryGetValue(uiName, out var uiBase))
            uiBase.OnDestroy();
    }
}

/// <summary>
/// UI面板基类
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    public EUIType type = EUIType.Normal; //窗口的位置
    public EUIMode mode = EUIMode.Normal; //窗口显示类型

    public virtual void OnCreate()
    {
    }

    public virtual void OnEnable()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnDisable()
    {
        this.gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
    }
}