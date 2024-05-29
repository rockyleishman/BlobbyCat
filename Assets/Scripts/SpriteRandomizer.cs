using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    protected SpriteRenderer _renderer;

    [SerializeField] public Sprite[] Sprites;
    [SerializeField] public bool RandomXFlip = true;

    private void Start()
    {
        //init fields
        _renderer = GetComponent<SpriteRenderer>();

        //randomize sprite
        RandomizeSprite();
    }

    protected virtual void RandomizeSprite()
    {
        if (Sprites.Length > 0)
        {
            _renderer.sprite = Sprites[Random.Range(0, Sprites.Length)];
        }
        if (RandomXFlip && Random.value > 0.5f)
        {
            _renderer.flipX = !_renderer.flipX;
        }
    }
}
