using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private int _MaxVolume = 10;
    [SerializeField] private List<SoundEffectQueue> _soundEffectQueuePrefabs;

    private List<SoundEffectQueue> _soundEffectQueueObjects;

    private void Start()
    {
        //set mixer volume levels based on game settings
        SetMasterVolume(DataManager.Instance.GameSettingsObject.MasterVolume);
        SetMusicVolume(DataManager.Instance.GameSettingsObject.MusicVolume);
        SetSFXVolume(DataManager.Instance.GameSettingsObject.SFXVolume);

        //instantiate sound effect queues
        _soundEffectQueueObjects = new List<SoundEffectQueue>();
        foreach (SoundEffectQueue sfxQueuePrefab in _soundEffectQueuePrefabs)
        {
            SoundEffectQueue sfxQueueObject = Instantiate(sfxQueuePrefab);
            sfxQueueObject.name = sfxQueuePrefab.name;
            sfxQueueObject.transform.parent = transform;
            _soundEffectQueueObjects.Add(sfxQueueObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume / _MaxVolume, 0.0001f, 1.0f)) * 20.0f);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume / _MaxVolume, 0.0001f, 1.0f)) * 20.0f);
    }

    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume / _MaxVolume, 0.0001f, 1.0f)) * 20.0f);
    }

    public void PlaySFX(string name)
    {
        foreach (SoundEffectQueue sfxQueue in _soundEffectQueueObjects)
        {
            if (sfxQueue.EnqueueIfName(name))
            {
                return;
            }
        }

        //if no sound found by name
        throw new NoSFXFoundException();
    }
}
