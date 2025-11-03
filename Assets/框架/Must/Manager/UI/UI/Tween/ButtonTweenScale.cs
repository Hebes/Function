// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
// using zhaorh.UI;
//
// // <summary>
// // @Author: zrh
// // @Date: 2022,12,28,19:24
// // @Description:
// // </summary>
//
// namespace Game.UI
// {
//     /// <summary>
//     /// 按钮动画
//     /// </summary>
//     [AddComponentMenu("Tween/ButtonTweenScale")]
//     public class ButtonTweenScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
//     {
//         // 最小scal
//         public Vector2 change = new Vector2(.8f, .8f);
//         public float duration = 0.2f;
//         /// <summary>
//         /// For UiPuddingEffect
//         /// </summary>
//         public bool enablePuddingEffect = false;
//         UiPuddingEffect uipuddingEfect = null;
//         public UiPuddingEffect.Type pivotType;
//         public float attenCoefficient = 0.2f;
//         public float R = 0.3f;
//         public float puddingT = 0.5f;
//         public float T = 1f;
//
//         public List<Transform> candiates = new List<Transform>();
//
//         [SerializeField]
//         float cutOffR = 0.1f;
//         private Vector3 originScale;
//         private Vector3 changedScale
//         {
//             get
//             {
//                 var endVector = new Vector3(Mathf.Abs(change.x) * originScale.x, Mathf.Abs(change.y) * originScale.y, originScale.z);
//                 return endVector;
//             }
//         }
//
//         private RectTransform animTargetTransform;
//         private Button btn;
//         private bool hasMakeSameAnchor;
//
//         // init
//         private void Awake()
//         {
//             originScale = transform.localScale;
//             originScale.x = Mathf.Abs(originScale.x);
//             originScale.y = Mathf.Abs(originScale.y);
//             originScale.z = Mathf.Abs(originScale.z);
//         }
//
//         private void MakeCopyForAnimation()
//         {
//             btn = GetComponent<Button>();
//             if (btn != null && btn.targetGraphic != null)
//             {
//                 var orignalImg = btn.targetGraphic.gameObject.GetComponent<Image>();
//
//                 var imgHostGo = new GameObject();
//                 imgHostGo.name = "ButtonScaleImage";
//                 imgHostGo.transform.SetParent(btn.transform);
//                 imgHostGo.transform.localPosition = Vector3.zero;
//                 imgHostGo.transform.localScale = btn.transform.localScale;
//                 imgHostGo.transform.SetAsFirstSibling();
//
//                 if (orignalImg != null && orignalImg.sprite != null)
//                 {
//                     var copyImg = imgHostGo.AddComponent<Image>();
//                     copyImg.rectTransform.anchorMin = Vector2.zero;
//                     copyImg.rectTransform.anchorMax = Vector2.one;
//                     copyImg.rectTransform.sizeDelta = Vector2.zero;
//                     copyImg.sprite = orignalImg.sprite;
//                     copyImg.type = orignalImg.type;
//                     copyImg.color = orignalImg.color;
//                     orignalImg.color = new Color(1, 1, 1, 0);
//                     copyImg.raycastTarget = false;
//                     animTargetTransform = copyImg.rectTransform;
//
//                     var follower = imgHostGo.AddComponent<ButtonTweenScaleFollower>();
//                     follower.SetContent(orignalImg, copyImg);
//                 }
//                 else
//                 {
//                     animTargetTransform = imgHostGo.AddComponent<RectTransform>();
//                 }
//
//                 if(enablePuddingEffect && orignalImg != null)
//                 {
//                     uipuddingEfect = imgHostGo.AddComponent<UiPuddingEffect>();
//                     uipuddingEfect.pivotType = pivotType;
//                     uipuddingEfect.puddingT = puddingT;
//                     uipuddingEfect.R = R;
//                     uipuddingEfect.attenCoefficient = attenCoefficient;
//                     uipuddingEfect.T = T;
//                 }
//             }
//         }
//
//         private void MakeSameAnchor()
//         {
//             if (btn != null && btn.targetGraphic != null && !enablePuddingEffect)
//             {
//                 if (animTargetTransform == null)
//                 {
//                     return;
//                 }
//                 var size = animTargetTransform.rect.size;
//                 animTargetTransform.anchorMin = btn.targetGraphic.rectTransform.anchorMin;
//                 animTargetTransform.anchorMax = btn.targetGraphic.rectTransform.anchorMax;
//                 animTargetTransform.pivot = btn.targetGraphic.rectTransform.pivot;
//                 animTargetTransform.sizeDelta = size;
//                 animTargetTransform.SetCenterAtTargetCenter(btn.targetGraphic.rectTransform);
//                 hasMakeSameAnchor = true;
//
//                 SetBrotherAsChild();
//             }
//         }
//
//         private void SetBrotherAsChild()
//         {
//             if(!enablePuddingEffect)
//             {
//                 List<Transform> candiates = new List<Transform>();
//                 for (var i = 0; i < btn.transform.childCount; i++)
//                 {
//                     var candidate = btn.transform.GetChild(i);
//                     if (candidate != animTargetTransform)
//                     {
//                         var tirgger = candidate.GetComponent<GuideTriggerBase>();
//                         if (tirgger == null)
//                         {
//                             candiates.Add(candidate);
//                         }
//                     }
//                 }
//
//                 for (var i = 0; i < candiates.Count; i++)
//                 {
//                     candiates[i].SetParent(animTargetTransform);
//                 }
//             }
//         }
//
//         // 处理两个按钮同时点击 切换切换时 按钮无 point up 响应情况
//         private void OnEnable()
//         {
//             if (animTargetTransform == null)
//             {
//                 MakeCopyForAnimation();
//             }
//             if (originScale.x > 0 && animTargetTransform != null) // 排除比awake 先 执行情况
//             {
//                 animTargetTransform.localScale = originScale;
//             }
//             if(enablePuddingEffect && uipuddingEfect != null)
//             {
//                 uipuddingEfect.enabled = true;
//             }
//         }
//
//         // Down
//         public void OnPointerDown(PointerEventData eventData)
//         {
//             if (hasMakeSameAnchor == false)
//             {
//                 MakeSameAnchor();
//             }
//
//             if (animTargetTransform != null)
//             {
//                 if(enablePuddingEffect)
//                 {
//                     transform.DOScale(changedScale, duration)
//                     .ChangeStartValue(originScale)
//                     .SetUpdate(true)
//                     .SetEase(Ease.Linear);
//                 }
//                 else
//                 {
//                     //缩放是按比例来的，因此不可能出现负数
//                     animTargetTransform.DOScale(changedScale, duration)
//                         .ChangeStartValue(originScale)
//                         .SetUpdate(true)
//                         .SetEase(Ease.Linear);
//                 }
//             }
//         }
//
//         // Up
//         public void OnPointerUp(PointerEventData eventData)
//         {
//             if (animTargetTransform != null)
//             {
//                 if (enablePuddingEffect)
//                 {
//                     transform.DOScale(originScale, duration)
//                     .ChangeStartValue(changedScale)
//                     .SetUpdate(true)
//                     .SetEase(Ease.Linear);
//                     
//                 }
//                 else
//                 {
//                     animTargetTransform.DOScale(originScale, duration)
//                     .ChangeStartValue(changedScale)
//                     .SetUpdate(true)
//                     .SetEase(Ease.Linear);
//                 }
//             }
//         }
//     }
// }