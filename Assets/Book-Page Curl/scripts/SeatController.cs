using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeatController : MonoBehaviour
{
    public TMP_Text playerName;
    public Image image_ready;
    public Image image_notReady;
    public Button button;
    public Image image_background;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
/*        image_ready.enabled = false;
        image_notReady.enabled = false;
        image_background.enabled = false;*/
    }
}
