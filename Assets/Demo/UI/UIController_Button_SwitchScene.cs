using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Button_SwitchScene : MonoBehaviour
{
    public string sceneNameToChange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneModule.LoadScene(sceneNameToChange));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
