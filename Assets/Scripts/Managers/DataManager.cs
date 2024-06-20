using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] public PlayerValues PlayerValuesObject;
    [SerializeField] public PlayerStatus PlayerStatusObject;
    [SerializeField] public GameStatus GameStatusObject;
    [SerializeField] public LevelCollectionData LevelCollectionDataObject;
}
