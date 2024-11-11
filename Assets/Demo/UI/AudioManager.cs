using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float soundRatio = 0.7f;
    public static float musicRatio = 0.1f;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundRatio = PlayerPrefs.GetFloat("Sound");
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            soundRatio = PlayerPrefs.GetFloat("Music");
        }
        ChangeMusic(musicRatio);
        ChangeSound(soundRatio);
    }
    public static void ChangeSound(float value)
    {
        soundRatio = value;
        PlayerPrefs.SetFloat("Sound", value) ;
    }

    public static void ChangeMusic(float value)
    {
        musicRatio = value;
        PlayerPrefs.SetFloat("Music", value);
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.volume = musicRatio;
        }
    }
    

}
