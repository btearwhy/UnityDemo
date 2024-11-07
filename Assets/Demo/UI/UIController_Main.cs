using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController_Main : MonoBehaviour
{
    public string sceneName;
    public Button buttonStart;
    public Button buttonSettings;
    public GameObject Setting;
    public Button buttonQuit;
    // Start is called before the first frame update
    void Start()
    {
        buttonStart.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        });

        buttonQuit.onClick.AddListener(Application.Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
