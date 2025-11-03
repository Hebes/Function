using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,23,16:33
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// UGUI 降低填充率技巧两则
    /// Empty4 raycast. see http://blog.uwa4d.com/archives/fillrate.html
    /// </summary>
    public class Empty4Raycast : MaskableGraphic
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
    
    
}