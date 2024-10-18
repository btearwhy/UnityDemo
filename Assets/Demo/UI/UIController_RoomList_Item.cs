using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController_RoomList_Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void setItem(string name, string v, bool buttonEnable)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = name;
        transform.GetChild(1).GetComponent<TMP_Text>().text = v;
        transform.GetChild(2).GetComponent<Button>().interactable = buttonEnable;
    }
}
