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
    public Button buttonQuit;
    // Start is called before the first frame update
    void Start()
    {
        buttonStart.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        });
        buttonSettings.onClick.AddListener(() =>
        {
            GameObject gameObject = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "Canvas_Settings"));
            gameObject.SetActive(true);
        });
        buttonQuit.onClick.AddListener(Application.Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
