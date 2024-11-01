using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
