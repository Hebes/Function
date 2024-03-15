using UnityEngine;

/// <summary>
/// 触发激活
/// </summary>
public class ActivateTrigger : MonoBehaviour
{
    public enum Mode
    {
        Trigger = 0, // Just broadcast the action on to the target 把行动广播给目标

        Replace = 1, // replace target with source	用源替换目标

        Activate = 2, // Activate the target GameObject 激活目标GameObject

        Enable = 3, // Enable a component	启用组件

        Animate = 4, // Start animation on target	在目标上启动动画

        Deactivate = 5 // Decativate target GameObject 指定目标GameObject
    }

    /// <summary>
    /// The action to accomplish
    /// 要完成的行动
    /// </summary>
    public Mode action = Mode.Activate;

   /// <summary>
   /// The game object to affect. If none, the trigger work on this game object
   /// 要影响的游戏对象。如果没有，触发器对这个游戏对象起作用
   /// </summary>
    public Object target;

    public GameObject source;

    public int triggerCount = 1;

    ///
    public bool repeatTrigger = false;

    void DoActivateTrigger()
    {
        triggerCount--;

        if (triggerCount == 0 || repeatTrigger)
        {
            Object currentTarget = target != null ? target : gameObject;
            Behaviour targetBehaviour = currentTarget as Behaviour;
            GameObject targetGameObject = currentTarget as GameObject;
            if (targetBehaviour != null)
                targetGameObject = targetBehaviour.gameObject;

            switch (action)
            {
                case Mode.Trigger:
                    targetGameObject.BroadcastMessage("DoActivateTrigger");
                    break;
                case Mode.Replace:
                    if (source != null)
                    {
                        Object.Instantiate(source, targetGameObject.transform.position, targetGameObject.transform.rotation);
                        DestroyObject(targetGameObject);
                    }

                    break;
                case Mode.Activate:
                    targetGameObject.SetActive(true);
                    break;
                case Mode.Enable:
                    if (targetBehaviour != null)
                        targetBehaviour.enabled = true;
                    break;
                case Mode.Animate:
                    targetGameObject.GetComponent<Animation>().Play();
                    break;
                case Mode.Deactivate:
                    targetGameObject.SetActive(false);
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DoActivateTrigger();
    }
}