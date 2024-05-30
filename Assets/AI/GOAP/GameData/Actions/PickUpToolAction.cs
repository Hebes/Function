using System;
using UnityEngine;

/// <summary>
/// 拾取工具
/// </summary>
public class PickUpToolAction : GoapAction
{
    private bool hasTool = false;
    private SupplyPileComponent targetSupplyPile; // where we get the tool from 我们从哪里得到这个工具

    public PickUpToolAction()
    {
        addPrecondition("hasTool", false); // don't get a tool if we already have one 如果我们已经有了工具，就不要再用了
        addEffect("hasTool", true); // we now have a tool 我们现在有了一个工具
    }


    public override void reset()
    {
        hasTool = false;
        targetSupplyPile = null;
    }

    public override bool isDone()
    {
        return hasTool;
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near a supply pile so we can pick up the tool 是的，我们得靠近补给堆，这样我们才能拿工具
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // find the nearest supply pile that has spare tools 找到最近有备用工具的补给堆
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(SupplyPileComponent));
        SupplyPileComponent closest = null;
        float closestDist = 0;

        foreach (SupplyPileComponent supply in supplyPiles)
        {
            if (supply.numTools > 0)
            {
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = supply;
                    closestDist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    // is this one closer than the last?
                    float dist = (supply.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        // we found a closer one, use it
                        closest = supply;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
            return false;

        targetSupplyPile = closest;
        target = targetSupplyPile.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        if (targetSupplyPile.numTools > 0)
        {
            targetSupplyPile.numTools -= 1;
            hasTool = true;

            // create the tool and add it to the agent 创建工具并将其添加到代理
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
            GameObject tool = Instantiate(prefab, transform.position, transform.rotation);
            backpack.tool = tool;
            tool.transform.parent = transform; // attach the tool 连接工具

            return true;
        }

        // we got there but there was no tool available! Someone got there first. Cannot perform action
        //我们到了那里，但是没有可用的工具!有人先到了。无法执行操作
        return false;
    }
}