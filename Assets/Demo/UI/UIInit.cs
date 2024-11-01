using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInit : MonoBehaviour
{
    public string MainUI;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", MainUI));
    }
}
