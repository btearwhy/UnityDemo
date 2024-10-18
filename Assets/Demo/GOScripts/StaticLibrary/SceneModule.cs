using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneModule
{
    public static void LoadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);    
    }    
}
