using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableContainer : MonoBehaviour
{
    [SerializeField] public Collectable[] Collectables;

    [Header("Explosion")]
    [SerializeField] public float MaxXSpeed = 2.0f;
    [SerializeField] public float XResistance = 1.0f;
    [SerializeField] public float MinYVelocity = 2.5f;
    [SerializeField] public float MaxYVelocity = 5.0f;
    [SerializeField] public float Gravity = 10.0f;
    [SerializeField] public float TerminalVelocity = 10.0f;

    public void DropCollectables()
    {
        foreach (Collectable item in Collectables)
        {
            item.transform.position = transform.position;
            item.Launch(new Vector2(Random.Range(-MaxXSpeed, MaxXSpeed), Random.Range(MinYVelocity, MaxYVelocity)), Gravity, XResistance, TerminalVelocity);
        }
    }
}
