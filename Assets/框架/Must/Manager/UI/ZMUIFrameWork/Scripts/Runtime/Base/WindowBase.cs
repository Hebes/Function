// using System;
// using DG.Tweening;
// using UnityEngine;
//
// namespace Framework.Core
// {
//     public class WindowBase : MonoBehaviour
//     {
//         public Canvas Canvas { get; set; }
//         public bool Visible { get; set; }
//         public bool PopStack { get; set; } //是否是通过堆栈系统弹出的弹窗
//
//         public Action<WindowBase> PopStackListener { get; set; }
//
//         /// <summary>
//         /// 与Mono Awake调用时机和次数保持一致
//         /// </summary>
//         public virtual void OnAwake()
//         {
//         }
//
//         /// <summary>
//         /// 与MonoOnEnable一致
//         /// </summary>
//         public virtual void OnShow()
//         {
//         }
//
//         protected virtual void OnUpdate()
//         {
//         }
//
//         /// <summary>
//         /// 与Mono OnDisable 一致
//         /// </summary>
//         public virtual void OnHide()
//         {
//         }
//
//         /// <summary>
//         /// 在当前界面被销毁时调用一次
//         /// </summary>
//         public virtual void OnDestroy()
//         {
//         }
//
//
//
//         protected void HideWindow<T>()where T : WindowBase => CoreUI.Instance.HideWindow<T>();
//         protected T PopUpWindow<T>() where T : WindowBase => CoreUI.Instance.PopUpWindow<T>();
//         protected T GetWindow<T>() where T : WindowBase => CoreUI.Instance.GetWindow<T>();
//     }
// }