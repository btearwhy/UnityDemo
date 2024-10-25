using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DontDestroyOnLoadManager
{
    public static List<GameObject> survivors = new List<GameObject>();

    public static void DontDestroyOnLoad(GameObject gameObject)
    {
        GameObject.DontDestroyOnLoad(gameObject);
        survivors.Add(gameObject);
    }

    public static void DestroyAll()
    {
        survivors.ForEach((go) => GameObject.Destroy(go));
        survivors.Clear();
    }

}
