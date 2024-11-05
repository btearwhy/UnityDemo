using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform Knob;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
