using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float soundRatio = 0.7f;
    public static float musicRatio = 0.3f;

    private void Start()
    {
        ChangeMusic(musicRatio);
    }
    public static void ChangeSound(float value)
    {
        soundRatio = value;

    }

    public static void ChangeMusic(float value)
    {
        musicRatio = value;

        foreach(AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.volume = musicRatio;
        }
    }
    

}
