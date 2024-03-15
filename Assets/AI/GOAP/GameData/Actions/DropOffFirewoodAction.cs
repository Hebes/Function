using System;
using UnityEngine;

/// <summary>
/// 送柴火行动
/// </summary>
public class DropOffFirewoodAction : GoapAction
{
    /// <summary>
    /// 送柴火
    /// </summary>
    private bool droppedOffFirewood = false;

    /// <summary>
    /// 我们把柴火扔在哪里
    /// where we drop off the firewood
    /// </summary>
    private SupplyPileComponent targetSupplyPile;

    public DropOffFirewoodAction()
    {
        //如果我们没有柴火就不能送来吗
        //can't drop off firewood if we don't already have some
        addPrecondition("hasFirewood", true);

        //我们现在没有柴火了
        //we now have no firewood
        addEffect("hasFirewood", false);

        //我们收集柴火
        //we collected firewood
        addEffect("collectFirewood", true);
    }


    public override void reset()
    {
        droppedOffFirewood = false;
        targetSupplyPile = null;
    }

    public override bool isDone()
    {
        return droppedOffFirewood;
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near a supply pile so we can drop off the firewood
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // find the nearest supply pile that has spare firewood
        SupplyPileComponent[] supplyPiles = (SupplyPileComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(SupplyPileComponent));
        SupplyPileComponent closest = null;
        float closestDist = 0;

        foreach (SupplyPileComponent supply in supplyPiles)
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

        if (closest == null)
            return false;

        targetSupplyPile = closest;
        target = targetSupplyPile.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetSupplyPile.numFirewood += backpack.numFirewood;
        droppedOffFirewood = true;
        backpack.numFirewood = 0;

        return true;
    }
}