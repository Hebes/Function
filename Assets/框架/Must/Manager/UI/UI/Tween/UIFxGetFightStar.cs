// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.UI;
// using zhaorh.UI;
//
// // <summary>
// // @Author: zrh
// // @Date: 2023,02,10,16:28
// // @Description:
// // </summary>
//
// namespace Game.UI
// {
//     public class UIFxGetFightStar : MonoBehaviour
//     {
//         public float duration = 0.4f;
//         public Ease ease = Ease.InOutCubic;
//
//         public Vector3 from = new Vector3(3, 3, 3);
//         public Vector2 to = new Vector3(1, 1, 1);
//
//         private void Start()
//         {
//             Stop();
//         }
//
//         [ContextMenu("Play")]
//         public void Play()
//         {
//             this.gameObject.SetActive(true);
//
//             var mySequence = DOTween.Sequence();
//             this.transform.localScale = from;
//             mySequence.SetEase(ease);
//             mySequence.Append(transform.DOScale(to, duration));
//
//             var fxAnim = transform.GetChildComponent<SpriteAnimation>("GetStar");
//             if (fxAnim != null) {
//                 fxAnim.Play ();
//             } else {
//                 Debug.LogError ("找不到SpriteAnimation控件");
//             }
//         }
//
//         [ContextMenu("Stop")]
//         public void Stop()
//         {
//             this.gameObject.SetActive(false);
//
//             var fxAnim = transform.GetChildComponent<SpriteAnimation>("GetStar");
//             if(fxAnim!=null){
//                 fxAnim.Stop();	
//             } else {
//                 Debug.LogError ("找不到SpriteAnimation控件");
//             }
//         }
//
//         public void SetColorIn()
//         {
//             //TODO 需要修改
//             //transform.GetChildComponent<Image>("Icon").sprite = ResMgr.Instance.GetSprite("ui_common_xing");
//         }
//
//         public void SetGreyOut()
//         {
//             //TODO 需要修改
//             //transform.GetChildComponent<Image>("Icon").sprite = ResMgr.Instance.GetSprite("ui_common_xing_hui");
//             gameObject.SetActive(true);
//         }
//     }
// }