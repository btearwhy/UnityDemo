using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInit : MonoBehaviour
{
    public string MainUI;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Setting = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "Canvas_Settings"));
        GameObject MainObj = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", MainUI));
        UIController_Main Main = MainObj.GetComponentInChildren<UIController_Main>();
        Main.Setting = Setting;
        Main.buttonSettings.onClick.AddListener(() =>
        {
            Setting.SetActive(!Setting.activeSelf);
        });
        Setting.SetActive(false);
    }
}
