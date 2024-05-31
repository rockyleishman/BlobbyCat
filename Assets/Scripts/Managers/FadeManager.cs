using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        //init fields
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void FlashIn()
    {
        StopAllCoroutines();
        _renderer.color = Color.clear;
    }

    public void FlashOut(Color colour)
    {
        StopAllCoroutines();
        _renderer.color = colour;
    }

    public void FadeIn(Color colour, float time)
    {
        StopAllCoroutines();
        StartCoroutine(OnFadeIn(colour, time));
    }

    private IEnumerator OnFadeIn(Color colour, float time)
    {
        float timer = 0.0f;

        while (timer < time)
        {
            timer += Time.deltaTime;

            _renderer.color = Color.Lerp(colour, Color.clear, timer / time);

            yield return null;
        }

        _renderer.color = Color.clear;
    }

    public void FadeOut(Color colour, float time)
    {
        StopAllCoroutines();
        StartCoroutine(OnFadeOut(colour, time));
    }

    private IEnumerator OnFadeOut(Color colour, float time)
    {
        float timer = 0.0f;

        while (timer < time)
        {
            timer += Time.deltaTime;

            _renderer.color = Color.Lerp(Color.clear, colour, timer / time);

            yield return null;
        }

        _renderer.color = colour;
    }
}
