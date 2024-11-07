﻿// jave.lin 2019.08.15
using UnityEngine;

namespace 镜面反射水面扰动
{
    public class UpdateProps : MonoBehaviour
    {
        private static int nHash;
        private static int pHash;

        public GameObject obj;
        public GameObject plane;

        private Material objMat;

        // Start is called before the first frame update
        void Start()
        {
            objMat = obj.GetComponent<MeshRenderer>().sharedMaterial;
            nHash = UnityEngine.Shader.PropertyToID("n");
            pHash = UnityEngine.Shader.PropertyToID("p");
        }

        // Update is called once per frame
        void Update()
        {
            // n==normal of plane
            // p==position of plane
            var n = plane.transform.up;
            var p = plane.transform.position;

            objMat.SetVector(nHash, n);
            objMat.SetVector(pHash, p);
        }
    }
}