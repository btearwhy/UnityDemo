using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : Page
{
    public Button buttonBack;
    public TMP_Dropdown dropdownLanguage;
    public Slider sliderMusic;
    public Slider sliderSound;

    void Start()
    {

        GetComponent<Canvas>().worldCamera = Camera.main;

        buttonBack.onClick.AddListener(() =>
        {
            FlipBack();
            PlayerController pc = PlayerState.GetInstance().GetController();
            if (pc)
            {
                pc.enabled = true;
            }
        });

        dropdownLanguage.ClearOptions();
        IEnumerable<TMP_Dropdown.OptionData> characterOptions = from language in System.Enum.GetNames(typeof(LanguageManager.LanguageType))
                                                                select new TMP_Dropdown.OptionData(language);
        dropdownLanguage.AddOptions(characterOptions.ToList());
        dropdownLanguage.onValueChanged.AddListener((nr) =>
        {
            LanguageManager.ChangeLanguage((LanguageManager.LanguageType)nr);
        }
        );

        sliderMusic.value = AudioManager.musicRatio;
        sliderMusic.onValueChanged.AddListener((value) =>
        {
            AudioManager.ChangeMusic(value);
        });

        sliderSound.value = AudioManager.soundRatio;
        sliderSound.onValueChanged.AddListener((value) =>
        {
            AudioManager.ChangeSound(value);
        });

    }
    public override void InitialOperation()
    {
        base.InitialOperation();

        LeftTopPage = LastPage.GetComponent<RectTransform>();
    }

    public override void LeaveLeftTop()
    {
        base.LeaveLeftTop();

        TurnPage.AutoFlip(FlipRegion.LeftTop);
    }
}
