using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapType : ScriptableObject
{
    public string sceneName;
    public GameObject scenePrefab;
    public string mapName;
    public Sprite image;
    public string introduction;
    public List<Vector3> spawnPositions;
    public AudioClip backgoundMusic;
}
