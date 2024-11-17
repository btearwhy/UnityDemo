public bool IsClickOnUI(Vector3 location)
{
    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    pointerEventData.position = location;

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerEventData, results);
    for (int i = 0; i < results.Count; i++)
    {
        if (results[i].gameObject.CompareTag("UI"))
        {
            return true;
        }
    }

    return false;
}