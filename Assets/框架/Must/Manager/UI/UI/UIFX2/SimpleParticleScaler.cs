using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,02,14,16:30
// @Description:
// </summary>

namespace Game.UI
{
    public class SimpleParticleScaler : MonoBehaviour
    {
        public float particleScale = 1.0f;

        public void SetParticleScale(float particleScale)
        {
            this.particleScale = particleScale;
            ScaleAllParticles();
        }

        private void Start()
        {
            ScaleAllParticles();
        }

        [ContextMenu("ScaleAllParticles")]
        private void ScaleAllParticles()
        {
            var pss = GetComponentsInChildren<ParticleSystem>(true);
            for (var i = 0; i < pss.Length; i++)
            {
                pss[i].transform.localScale = new Vector3(particleScale, particleScale, particleScale);
            }
        }
    }
}