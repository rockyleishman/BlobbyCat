using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    private Image _image;

    private void Start()
    {
        //init fields
        _image = GetComponent<Image>();
    }

    public void FlashIn()
    {
        StopAllCoroutines();
        _image.color = Color.clear;
    }

    public void FlashOut(Color colour)
    {
        StopAllCoroutines();
        _image.color = colour;
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

            _image.color = Color.Lerp(colour, Color.clear, timer / time);

            yield return null;
        }

        _image.color = Color.clear;
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

            _image.color = Color.Lerp(Color.clear, colour, timer / time);

            yield return null;
        }

        _image.color = colour;
    }
}
