﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
    private GameRoom gameRoom;
    private PlayerState playerState;
    private float remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;
        remainingTime = gameRoom.timeCondition;
        playerState = PlayerState.GetInstance();
        playerState.CreateHUD();
        playerState.SetCharacter(gameRoom.characters[gameRoom.chosenCharacter]);
        GameObject characterObj = playerState.SpawnCharacter();
        PlayerController controller = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("controllers", "PlayerController")).GetComponent<PlayerController>();
        playerState.AttachController(controller, characterObj);
        playerState.InitHUD();
    }



    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
        playerState.HUD.text_time.text = remainingTime.ToString();/*timeSpan.ToString("mm:ss");*/
    }
}
