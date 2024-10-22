using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class AssetDatabaseTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
/*        MapType map = AssetDatabase.LoadAssetAtPath<MapType>("Assets/Demo/Data/map.asset");
        if (map == null)
        {
            // Create and save ScriptableObject because it doesn't exist yet
            map = ScriptableObject.CreateInstance<MapType>();
            map.sceneName = "SampleScene";
            map.mapName = "map1";
            map.introduction = "test map";
            AssetDatabase.CreateAsset(map, "Assets/Demo/Data/map.asset");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
