using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public enum LanguageType
    {
        CN,
        EN
    }

    public static LanguageType type = LanguageType.CN;

    private void OnEnable()
    {
        Init();
    }

    public static void Init()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            string defaultLanguage = PlayerPrefs.GetString("Language");
            string[] langs = System.Enum.GetNames(typeof(LanguageType));
            foreach (string lang in langs)
            {
                if (lang.Equals(defaultLanguage))
                {
                    type = (LanguageType)System.Enum.Parse(typeof(LanguageType), lang);
                    break;
                }
            }
        }
    }

    internal static string GetById(int languageId)
    {
        return ((TextTable)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("languages", type.ToString())).texts[languageId];
    }

    public static void ChangeLanguage(LanguageType languageType)
    {
        if (type == languageType) return;
        type = languageType;

        UpdateLanguage();

        PlayerPrefs.SetString("Language", languageType.ToString());
    }

    public static void UpdateLanguage()
    {
        TranslatedText[] translatedTexts = FindObjectsOfType<TranslatedText>();
        foreach (TranslatedText translatedText in translatedTexts)
        {
            translatedText.SetText(GetById(translatedText.languageId));
        }
    }

    public static string Parse(string text)
    {
        return text;
    }
    
}
