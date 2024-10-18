using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInit : MonoBehaviour
{
    public GameObject MainUI;
    // Start is called before the first frame update
    void Start()
    {
        MainUI = Instantiate(MainUI);
        MainUI.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
