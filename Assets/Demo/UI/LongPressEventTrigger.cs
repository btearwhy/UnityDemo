using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float durationThreshold = 0.3f;

    public UnityEvent onLongPress = new UnityEvent();
    public UnityEvent onPressReleased = new UnityEvent();
    public UnityEvent onShortPress = new UnityEvent();

    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float timePressStarted;


    private void Update()
    {
        if (isPointerDown && !longPressTriggered)
        {
            if (Time.time - timePressStarted > durationThreshold)
            {
                longPressTriggered = true;
                onLongPress.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timePressStarted = Time.time;
        isPointerDown = true;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (Time.time - timePressStarted < durationThreshold)
        {
            onShortPress.Invoke();
        }
        else
        {
            onPressReleased.Invoke();
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}