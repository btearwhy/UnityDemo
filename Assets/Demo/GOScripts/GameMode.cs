using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameType
{
    TimeOut,
    TargetScore
}

public class GameMode : ScriptableObject
{
    // Start is called before the first frame update
    string modeName;
    GameType gameType;


}
