using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private static PlayerState playerState;

    public int score;
    private PlayerController playerController;
    private PlayerState(){}
    public static PlayerState GetInstance()
    {
        if(playerState == null)
        {
            playerState = new PlayerState();
        }
        return playerState;
    }

    public void SetController(PlayerController playerController)
    {
        GetInstance().playerController = playerController;
    }

    public static bool IsUnderControll(GameObject gameObject)
    {
        return gameObject == GetInstance().playerController.character;
    }
}
