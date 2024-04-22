using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewRuleTile2", menuName = "Tiles/RuleTile2", order = 0)]
public class RuleTile2 : RuleTile
{
    [SerializeField] public List<TileBase> BlendedTiles;
    [SerializeField] public List<TileBlendSet> TileBlendSets;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.This:
                return DoesBlendedContainOther(other) || base.RuleMatch(neighbor, other);

            case TilingRuleOutput.Neighbor.NotThis:
                return !DoesBlendedContainOther(other) && base.RuleMatch(neighbor, other);

            default:
                return base.RuleMatch(neighbor, other);
        }       
    }

    private bool DoesBlendedContainOther(TileBase other)
    {
        bool containsOther = BlendedTiles.Contains(other);
        if (TileBlendSets.Count > 0)
        {
            foreach (TileBlendSet set in TileBlendSets)
            {
                if (set.BlendedTiles.Contains(other))
                {
                    containsOther = true;
                }
            }
        }
        return containsOther;
    }
}