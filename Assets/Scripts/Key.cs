using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    protected override void Collect()
    {
        _levelCollectionData.KeysHeld++;

        base.Collect();
    }
}
