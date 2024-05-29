using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableSpriteRandomizer : SpriteRandomizer
{
    [SerializeField] public Sprite[] DamagedSprites;
    private Sprite _healedSprite;
    private Sprite _damagedSprite;

    [SerializeField, Range(0.0f, 1.0f)] public float DamagedSpriteHealth = 0.5f;

    protected override void RandomizeSprite()
    {
        if (Sprites.Length > 0)
        {
            int index = Random.Range(0, Sprites.Length);
            _healedSprite = Sprites[index];
            _damagedSprite = DamagedSprites[index];

            _renderer.sprite = _healedSprite;
        }
        if (RandomXFlip && Random.value > 0.5f)
        {
            _renderer.flipX = !_renderer.flipX;
        }
    }

    public void DamageSprite()
    {
        _renderer.sprite = _damagedSprite;
    }

    public void HealSprite()
    {
        _renderer.sprite = _healedSprite;
    }
}
