namespace 查找网格重建的具体UI元素
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.UI;
 
    public class NewBehaviourScript : MonoBehaviour {
 
        IList<ICanvasElement> mLayoutRebuildQueue;
        IList<ICanvasElement> mGraphicRebuildQueue;
 
        private void Awake()
        {
            var type = typeof(CanvasUpdateRegistry);
            var field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            mLayoutRebuildQueue = (IList<ICanvasElement>)field?.GetValue(CanvasUpdateRegistry.instance);
            field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            mGraphicRebuildQueue = (IList<ICanvasElement>)field?.GetValue(CanvasUpdateRegistry.instance);
        }
 
        private void Update()
        {
            for (var j = 0; j < mLayoutRebuildQueue.Count; j++)
            {
                var rebuild = mLayoutRebuildQueue[j];
                if (ObjectValidForUpdate(rebuild))
                    Debug.LogFormat("{0}引起网格重建", rebuild.transform.name);
            }
 
            for (var j = 0; j < mGraphicRebuildQueue.Count; j++)
            {
                var element = mGraphicRebuildQueue[j];
                if (ObjectValidForUpdate(element))
                    Debug.LogFormat("{0}引起{1}网格重建", element.transform.name, element.transform.GetComponent<Graphic>().canvas.name);
            }
        }
        private bool ObjectValidForUpdate(ICanvasElement element)
        {
            var valid = element != null;
 
            var isUnityObject = element is Object;
            //Here we make use of the overloaded UnityEngine.Object == null, that checks if the native object is alive.
            if (isUnityObject)
                valid = (element as Object) != null; 
 
            return valid;
        }
    }

}