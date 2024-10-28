using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Controller_Scoreboard : MonoBehaviour
{
    public GameObject content;
    public string itemName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddItem(string nickName, int score)
    {
        GameObject item = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", itemName));
        item.GetComponentInChildren<TMP_Text>().text = nickName + "\t" + score;
        item.transform.SetParent(gameObject.transform);
    }
}
