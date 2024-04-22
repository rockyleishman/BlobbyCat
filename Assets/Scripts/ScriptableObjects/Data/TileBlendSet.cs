using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileBlendSetObject", menuName = "Tiles/TileBlendSet", order = 0)]
public class TileBlendSet : ScriptableObject
{
    [SerializeField] public List<TileBase> BlendedTiles;
}
