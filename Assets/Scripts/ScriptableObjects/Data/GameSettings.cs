using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English
}

[CreateAssetMenu(fileName = "GameSettingsObject", menuName = "Data/GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    [SerializeField, Range(0, 10)] public float MasterVolume = 10.0f;
    [SerializeField, Range(0, 10)] public float MusicVolume = 5.0f;
    [SerializeField, Range(0, 10)] public float SFXVolume = 5.0f;
    [SerializeField] public Language SelectedLanguage = Language.English;
    [SerializeField] public Language[] LanguageOptions = { Language.English };
}
