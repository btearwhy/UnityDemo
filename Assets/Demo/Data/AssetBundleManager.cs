using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class AssetBundleManager : ScriptableObject
{
    private AssetBundleManager() { 
        resourceMap = new Dictionary<string, AssetBundle>();
        assetBundleNames = new List<string>();
    }
        
    private static AssetBundleManager assetBundleManager;


    public Dictionary<string, AssetBundle> resourceMap;

    public List<string> assetBundleNames;
    public string basePath;
    // Start is called before the first frame update
    public static AssetBundleManager GetInstance() {
        if(assetBundleManager == null)
        {
            assetBundleManager = CreateInstance<AssetBundleManager>();
            assetBundleManager.assetBundleNames.Add("abilities");
            assetBundleManager.assetBundleNames.Add("maps");
            assetBundleManager.assetBundleNames.Add("characters");
            assetBundleManager.assetBundleNames.Add("controllers");
            assetBundleManager.assetBundleNames.Add("lines");
            assetBundleManager.assetBundleNames.Add("ui");
            assetBundleManager.assetBundleNames.Add("effects");
            assetBundleManager.assetBundleNames.Add("buffs");
            assetBundleManager.assetBundleNames.Add("elements");
            assetBundleManager.assetBundleNames.Add("languages");
            assetBundleManager.assetBundleNames.Add("levels");
            assetBundleManager.basePath = Application.streamingAssetsPath;
            assetBundleManager.Init();
        }
        return assetBundleManager;
    }
    private void Init()
    {
        foreach (string assetBundleName in assetBundleNames)
        {
            resourceMap.Add(assetBundleName, LoadAssetBundleFromFile(Path.Combine(basePath, assetBundleName)));
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private AssetBundle LoadAssetBundleFromFile(string assetBundlePath)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        if (assetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
        }
        return assetBundle;
    }

   
    public T LoadAsset<T>(string assetBundleName, string assetName) where T:Object
    {
        AssetBundle assetBundle = (AssetBundle)resourceMap[assetBundleName];
        if (assetBundle == null)
        {
            Debug.Log("No assetbundle under name + " + assetBundleName + "!");
        }
        return assetBundle.LoadAsset<T>(assetName) ;
    }

    public T[] LoadAssets<T>(string assetBundleName) where T : Object
    {
        AssetBundle assetBundle = (AssetBundle)resourceMap[assetBundleName];
        if (assetBundle == null)
        {
            Debug.Log("No assetbundle under name + " + assetBundleName + "!");
        }
        return assetBundle.LoadAllAssets<T>();
    }
}
