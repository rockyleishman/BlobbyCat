using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectQueue : MonoBehaviour
{
    [SerializeField] private float _repeatTime = 0.1f;
    [SerializeField] private float _maxDelay = 2.0f;
    [SerializeField] private bool _playInRealTime = false;

    private AudioSource _audioSource;

    private int _queueLength;
    private bool _isPlaying;

    private void Start()
    {
        //init fields
        _audioSource = GetComponent<AudioSource>();
        _queueLength = 0;
        _isPlaying = false;
    }

    internal bool EnqueueIfName(string name)
    {
        if (gameObject.name == name)
        {
            Enqueue();
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void Enqueue()
    {
        if (_repeatTime * _queueLength < _maxDelay)
        {
            _queueLength++;
        }

        if (!_isPlaying)
        {
            StartCoroutine(PlayQueue());
        }
    }

    internal void ClearQueue()
    {
        _queueLength = 0;
    }

    private IEnumerator PlayQueue()
    {
        _isPlaying = true;

        while (_queueLength > 0)
        {
            _audioSource.Stop();
            _audioSource.Play();
            _queueLength--;

            if (_playInRealTime)
            {
                yield return new WaitForSecondsRealtime(_repeatTime);
            }
            else
            {
                yield return new WaitForSeconds(_repeatTime);
            }
        }

        _isPlaying = false;
    }
}
