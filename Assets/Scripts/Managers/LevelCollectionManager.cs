using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollectionManager : Singleton<LevelCollectionManager>
{
    private void Start()
    {
        //start with 0 keys
        DataManager.Instance.LevelCollectionDataObject.KeysHeld = 0;
    }
}
