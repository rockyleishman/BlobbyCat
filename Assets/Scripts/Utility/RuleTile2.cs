using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewRuleTile2", menuName = "Tiles/RuleTile2", order = 0)]
public class RuleTile2 : RuleTile
{
    [SerializeField] public List<TileBase> BlendedTiles;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.This:
                return BlendedTiles.Contains(other) || base.RuleMatch(neighbor, other);

            case TilingRuleOutput.Neighbor.NotThis:
                return !BlendedTiles.Contains(other) && base.RuleMatch(neighbor, other);

            default:
                return base.RuleMatch(neighbor, other);
        }       
    }
}