using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Button_PopUpCanvas : MonoBehaviour
{
    public GameObject PopUpWindow;
    private bool instantiated;
    // Start is called before the first frame update
    void Start()
    {
        instantiated = false;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!instantiated)
            {
                Instantiate(PopUpWindow);
                instantiated = true;
            }
            PopUpWindow.SetActive(true);
        });
    }
}
