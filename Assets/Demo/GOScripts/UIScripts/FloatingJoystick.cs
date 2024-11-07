using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform Knob;
    public Vector2 joyStickSize;
    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Knob = transform.GetChild(0).GetComponent<RectTransform>();
        gameObject.SetActive(false);
        joyStickSize = new Vector2(RectTransform.sizeDelta.x * RectTransform.localScale.x, RectTransform.sizeDelta.y * RectTransform.localScale.y);

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
