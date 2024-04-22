using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] public Sprite[] Sprites;
    [SerializeField] public bool RandomXFlip = true;

    private void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (Sprites.Length > 0)
        {
            renderer.sprite = Sprites[Random.Range(0, Sprites.Length)];
        }
        if (RandomXFlip && Random.value > 0.5f)
        {
            renderer.flipX = !renderer.flipX;
        }
    }
}
