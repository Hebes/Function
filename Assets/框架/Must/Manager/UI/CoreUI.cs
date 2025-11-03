// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Framework.Core
// {
//     
//     public class CoreUI : ICore
//     {
//         public static CoreUI Instance;
//
//         public void Init()
//         {
//             Instance = this;
//             GameObject gameObjectMemory = GameSetting.UICanvasPath.Load<GameObject>();
//             GameObject gameObjectTemp = Object.Instantiate(gameObjectMemory);
//             gameObjectTemp.name = gameObjectMemory.name;
//             UIComponent uiComponent = gameObjectTemp.GetComponent<UIComponent>();
//             _uiCamera = uiComponent.GetComponent<Camera>("T_UICamera");
//             _uiRoot = _uiCamera.transform;
//             Object.DontDestroyOnLoad(gameObjectTemp);
//         }
//
//         public IEnumerator AsyncEnter()
//         {
//             yield break;
//         }
//
//         public IEnumerator Exit()
//         {
//             yield break;
//         }
//
//         /// <summary>
//         /// 是否启用单遮模式 "窗口遮罩模式",
//         /// "True：开启单遮罩模式(多个窗口叠加只有一个Mask遮罩，透明度唯一)
//         /// False:开启叠着模式(一个窗口一个单独的Mask遮罩，透明度叠加)"
//         /// </summary>
//         public bool SINGMASK_SYSTEM = true;
//
//         /// <summary>
//         /// UI摄像机
//         /// </summary>
//         private Camera _uiCamera;
//
//         /// <summary>
//         /// UI挂载节点
//         /// </summary>
//         private Transform _uiRoot;
//
//         /// <summary>
//         /// 所有窗口的Dic
//         /// </summary>
//         private readonly Dictionary<string, WindowBase> _allWindowDic = new Dictionary<string, WindowBase>();
//
//         /// <summary>
//         /// 所有窗口的列表
//         /// </summary>
//         private readonly List<WindowBase> _allWindowList = new List<WindowBase>();
//
//         /// <summary>
//         /// 所有可见窗口的列表 
//         /// </summary>
//         private readonly List<WindowBase> _visibleWindowList = new List<WindowBase>();
//
//         private readonly Queue<WindowBase> _windowStack = new Queue<WindowBase>(); // 队列， 用来管理弹窗的循环弹出
//         private bool _startPopStackWndStatus = false; //开始弹出堆栈的表只，可以用来处理多种情况，比如：正在出栈种有其他界面弹出，可以直接放到栈内进行弹出 等
//
//         #region 窗口管理
//
//         /// <summary>
//         /// 只加载物体，不调用生命周期
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         public void PreLoadWindow<T>() where T : WindowBase
//         {
//             string wndName = typeof(T).Name;
//             //生成对应的窗口预制体
//             GameObject nWnd = LoadWindow(wndName);
//             T windowBase = nWnd.GetComponent<T>();
//             //2.初始出对应管理类
//             windowBase ??= nWnd.AddComponent<T>();
//             windowBase.Canvas = nWnd.GetComponent<Canvas>();
//             windowBase.Canvas.worldCamera = _uiCamera;
//             windowBase.OnAwake();
//             RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
//             rectTrans.anchorMax = Vector2.one;
//             rectTrans.offsetMax = Vector2.zero;
//             rectTrans.offsetMin = Vector2.zero;
//             _allWindowDic.Add(wndName, windowBase);
//             _allWindowList.Add(windowBase);
//             //$"预加载窗口 窗口名字：{wndName}".Log();
//         }
//
//         public T PopUpWindow<T>() where T : WindowBase
//         {
//             string wndName = typeof(T).Name;
//             var wnd = _allWindowDic.GetValueOrDefault(wndName);
//             if (wnd) return ShowWindow(wndName) as T;
//             $"请先PreLoadWindow窗口{wndName}".Error();
//             return InitializeWindow(wnd, wndName) as T;
//         }
//
//         public T GetWindow<T>() where T : WindowBase
//         {
//             foreach (WindowBase item in _visibleWindowList)
//             {
//                 if (item is T @base)
//                     return @base;
//             }
//
//             $"该窗口没有获取到：{typeof(T).Name}".Error();
//             return null;
//         }
//
//         public void HideWindow<T>() where T : WindowBase
//         {
//             string wndName = typeof(T).Name;
//             WindowBase window = _allWindowDic.GetValueOrDefault(wndName);
//             if (window != null && window.Visible)
//             {
//                 _visibleWindowList.Remove(window);
//                 SetWidnowMaskVisible();
//                 window.OnHide();
//                 window.Visible = false;
//                 window.gameObject.SetActive(false);
//             }
//
//             //在出栈的情况下，上一个界面隐藏时，自动打开栈种的下一个界面
//             PopNextStackWindow(window);
//         }
//
//         /// <summary>
//         /// 显示窗口
//         /// </summary>
//         /// <param name="winName"></param>
//         /// <returns></returns>
//         private WindowBase ShowWindow(string winName)
//         {
//             if (!_allWindowDic.ContainsKey(winName))
//                 $"{winName} 窗口不存在，请调用PopUpWindow 进行弹出".Error();
//             var window = _allWindowDic[winName];
//             if (!window.Visible)
//             {
//                 _visibleWindowList.Add(window);
//                 window.transform.SetAsLastSibling();
//                 SetWidnowMaskVisible();
//                 window.Visible = true;
//                 window.gameObject.SetActive(true);
//             }
//
//             window.OnShow();
//             return window;
//         }
//
//         /// <summary>
//         /// 销毁窗口
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         public void DestroyWinodw<T>() where T : WindowBase
//         {
//             string windowName = typeof(T).Name;
//             WindowBase window = _allWindowDic.GetValueOrDefault(windowName);
//             if (!window) return;
//             if (_allWindowDic.ContainsKey(windowName))
//             {
//                 _allWindowDic.Remove(windowName);
//                 _allWindowList.Remove(window);
//                 _visibleWindowList.Remove(window);
//             }
//
//             SetWidnowMaskVisible();
//             window.OnHide();
//             window.OnDestroy();
//             //ZMAsset.Release(window.gameObject, true);
//             //在出栈的情况下，上一个界面销毁时，自动打开栈种的下一个界面
//             PopNextStackWindow(window);
//             window = null;
//
//             Resources.UnloadUnusedAssets();
//         }
//
//         /// <summary>
//         /// 设置窗口遮罩开启
//         /// </summary>
//         private void SetWidnowMaskVisible()
//         {
//             if (!CoreUI.Instance.SINGMASK_SYSTEM) return;
//
//             WindowBase maxOrderWndBase = null; //最大渲染层级的窗口
//             int maxOrder = 0; //最大渲染层级
//             int maxIndex = 0; //最大排序下标 在相同父节点下的位置下标
//             //1.关闭所有窗口的Mask 设置为不可见
//             //2.从所有可见窗口中找到一个层级最大的窗口，把Mask设置为可见
//             for (int i = 0; i < _visibleWindowList.Count; i++)
//             {
//                 WindowBase window = _visibleWindowList[i];
//                 if (window == null) continue;
//                 if (maxOrderWndBase == null)
//                 {
//                     maxOrderWndBase = window;
//                     maxOrder = window.Canvas.sortingOrder;
//                     maxIndex = window.transform.GetSiblingIndex();
//                 }
//                 else
//                 {
//                     //找到最大渲染层级的窗口，拿到它
//                     if (maxOrder < window.Canvas.sortingOrder)
//                     {
//                         maxOrderWndBase = window;
//                         maxOrder = window.Canvas.sortingOrder;
//                     }
//                     //如果两个窗口的渲染层级相同，就找到同节点下最靠下一个物体，优先渲染Mask
//                     else if (maxOrder == window.Canvas.sortingOrder &&
//                              maxIndex < window.transform.GetSiblingIndex())
//                     {
//                         maxOrderWndBase = window;
//                         maxIndex = window.transform.GetSiblingIndex();
//                     }
//                 }
//             }
//         }
//
//         /// <summary>
//         /// 加载窗口
//         /// </summary>
//         /// <param name="wndNamePath"></param>
//         /// <returns></returns>
//         private GameObject LoadWindow(string wndNamePath)
//         {
//             string path = $"AssetsPackage/Prefab/UI/{wndNamePath}";
//             GameObject windowMemory = Resources.Load<GameObject>(path);
//             GameObject window = Object.Instantiate<GameObject>(windowMemory, _uiRoot);
//             window.transform.SetParent(_uiRoot);
//             window.transform.localScale = Vector3.one;
//             window.transform.localPosition = Vector3.zero;
//             window.transform.rotation = Quaternion.identity;
//             window.name = wndNamePath;
//             return window;
//         }
//
//         /// <summary>
//         /// 初始化窗口
//         /// </summary>
//         /// <param name="windowBase"></param>
//         /// <param name="wndName"></param>
//         /// <returns></returns>
//         private WindowBase InitializeWindow(WindowBase windowBase, string wndName)
//         {
//             //1.生成对应的窗口预制体
//             GameObject nWnd = LoadWindow(wndName);
//             //2.初始出对应管理类
//             if (nWnd != null)
//             {
//                 windowBase.Canvas = nWnd.GetComponent<Canvas>();
//                 windowBase.Canvas.worldCamera = _uiCamera;
//                 windowBase.transform.SetAsLastSibling();
//                 windowBase.OnAwake();
//                 windowBase.OnShow();
//                 RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
//                 rectTrans.anchorMax = Vector2.one;
//                 rectTrans.offsetMax = Vector2.zero;
//                 rectTrans.offsetMin = Vector2.zero;
//                 //增强代码鲁棒性 增加处理异常的健壮性
//                 if (_allWindowDic.ContainsKey(wndName))
//                 {
//                     if (_allWindowDic[wndName] != null)
//                     {
//                         //ZMAsset.Release(mAllWindowDic[wndName].gameObject, true);释放 回收到对象池 啥的
//                         _allWindowDic.Remove(wndName);
//                     }
//                     else
//                         _allWindowDic.Remove(wndName);
//
//                     if (_allWindowList.Contains(windowBase))
//                         _allWindowList.Remove(windowBase);
//                     if (_visibleWindowList.Contains(windowBase))
//                         _visibleWindowList.Remove(windowBase);
//                     Debug.LogError("mAllWindow Dic Alread Contains key:" + wndName);
//                 }
//
//                 _allWindowDic.Add(wndName, windowBase);
//                 _allWindowList.Add(windowBase);
//                 _visibleWindowList.Add(windowBase);
//                 SetWidnowMaskVisible();
//                 return windowBase;
//             }
//
//             Debug.LogError("没有加载到对应的窗口 窗口名字：" + wndName);
//             return null;
//         }
//
//         #endregion
//
//         #region 堆栈系统
//
//         /// <summary>
//         /// 进栈一个界面
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="popCallBack"></param>
//         public void PushWindowToStack<T>(Action<WindowBase> popCallBack = null) where T : WindowBase, new()
//         {
//             T wndBase = new T();
//             wndBase.PopStackListener = popCallBack;
//             _windowStack.Enqueue(wndBase);
//         }
//
//         /// <summary>
//         /// 弹出堆栈中第一个弹窗
//         /// </summary>
//         public void StartPopFirstStackWindow()
//         {
//             if (_startPopStackWndStatus) return;
//             _startPopStackWndStatus = true; //已经开始进行堆栈弹出的流程，
//             PopStackWindow();
//         }
//
//         /// <summary>
//         /// 压入并且弹出堆栈弹窗，若已弹出则只压入
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="popCallBack"></param>
//         public void PushAndPopStackWindow<T>(Action<WindowBase> popCallBack = null) where T : WindowBase, new()
//         {
//             PushWindowToStack<T>(popCallBack);
//             StartPopFirstStackWindow();
//         }
//
//         /// <summary>
//         /// 弹出堆栈中的下一个窗口
//         /// </summary>
//         /// <param name="windowBase"></param>
//         private void PopNextStackWindow(WindowBase windowBase)
//         {
//             if (windowBase == null) return;
//             if (!_startPopStackWndStatus) return;
//             if (!windowBase.PopStack) return;
//             windowBase.PopStack = false;
//             PopStackWindow();
//         }
//
//         /// <summary>
//         /// 弹出堆栈弹窗
//         /// </summary>
//         /// <returns></returns>
//         public bool PopStackWindow()
//         {
//             if (_windowStack.Count > 0)
//             {
//                 WindowBase window = _windowStack.Dequeue();
//                 WindowBase popWindow = PopUpWindow<WindowBase>();
//                 popWindow.PopStackListener = window.PopStackListener;
//                 popWindow.PopStack = true;
//                 popWindow.PopStackListener?.Invoke(popWindow);
//                 popWindow.PopStackListener = null;
//                 return true;
//             }
//
//             _startPopStackWndStatus = false;
//             return false;
//         }
//
//         /// <summary>
//         /// 清空栈数据
//         /// </summary>
//         public void ClearStackWindows()
//         {
//             _windowStack.Clear();
//         }
//
//         #endregion
//     }
//
//     /// <summary>
//     /// UI拓展
//     /// </summary>
//     public static class UIExpand
//     {
//         public static T PopUpWindow<T>() where T : WindowBase => CoreUI.Instance.PopUpWindow<T>();
//     }
// }