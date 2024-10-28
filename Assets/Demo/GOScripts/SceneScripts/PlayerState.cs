using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private GameRoom gameRoom;
    private static PlayerState playerState;

    public int score;
    private PlayerController playerController;
    private PlayerState() {
        gameRoom = GameRoom.gameRoom;
    }

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

    public PlayerController GetController()
    {
        return playerController;
    }
    public static bool IsUnderControll(GameObject gameObject)
    {
        return gameObject == GetInstance().playerController.character;
    }

    
}
