using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    private GameSettings _gameSettingsObject;

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TMP_Text _languageText;

    private void OnEnable()
    {
        _gameSettingsObject = DataManager.Instance.GameSettingsObject;

        //load settings
        _musicVolumeSlider.value = _gameSettingsObject.MusicVolume;
        _sfxSlider.value = _gameSettingsObject.SFXVolume;
        //TODO: set text based on language /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public void SetMasterVolume(float volume)
    {
        _gameSettingsObject.MusicVolume = volume;
        AudioManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        _gameSettingsObject.MusicVolume = volume;
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        _gameSettingsObject.SFXVolume = volume;
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void ChangeLanguage()
    {
        //find new language
        int newLanguageIndex = 0;
        for (int i = 0; i < _gameSettingsObject.LanguageOptions.Length; i++)
        {
            if (_gameSettingsObject.SelectedLanguage == _gameSettingsObject.LanguageOptions[i])
            {
                newLanguageIndex = i + 1;
                if (newLanguageIndex >= _gameSettingsObject.LanguageOptions.Length)
                {
                    newLanguageIndex = 0;
                }
                break;
            }
        }

        //change selected language
        _gameSettingsObject.SelectedLanguage = _gameSettingsObject.LanguageOptions[newLanguageIndex];

        //change displayed text in settings menu
        switch (_gameSettingsObject.SelectedLanguage)
        {
            case Language.English:
                _languageText.text = "ENGLISH";
                break;

            default:
                _languageText.text = "ERROR!";
                break;
        }
    }
}
