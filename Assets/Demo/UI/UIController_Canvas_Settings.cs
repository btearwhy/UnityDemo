using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIController_Canvas_Settings : MonoBehaviour
{
    // Start is called before the first frame update

    public Button buttonBack;
    public TMP_Dropdown dropdownLanguage;
    public Slider sliderMusic;
    public Slider sliderSound;

    void Start()
    {

        GetComponent<Canvas>().worldCamera = Camera.main;

        buttonBack.onClick.AddListener(() => gameObject.SetActive(false));

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
