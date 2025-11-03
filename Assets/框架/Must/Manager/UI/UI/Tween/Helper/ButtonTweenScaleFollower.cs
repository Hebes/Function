using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,28,19:27
// @Description:
// </summary>

namespace Game.UI
{
    public class ButtonTweenScaleFollower : MonoBehaviour
    {
        private Image orignalImg;
        private Image copyImg;

        public void SetContent(Image orignalImg, Image copyImg)
        {
            this.orignalImg = orignalImg;
            this.copyImg = copyImg;
        }

        private void Update()
        {
            if (orignalImg != null && copyImg != null)
            {
                //保持同步变灰或者是有关特效
                if (orignalImg.material != copyImg.material)
                {
                    copyImg.material = orignalImg.material;
                }

                if (orignalImg.enabled != copyImg.enabled)
                {
                    copyImg.enabled = orignalImg.enabled;
                }

                if (orignalImg.sprite != copyImg.sprite)
                {
                    copyImg.sprite = orignalImg.sprite;
                }
            }
        }
    }
}