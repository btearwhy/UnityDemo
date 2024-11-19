using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
    private GameRoom gameRoom;
    private PlayerState playerState;

    private GameObject levelMap;
    // Start is called before the first frame update
    void Start()
    {

        gameRoom = GameRoom.gameRoom;

        playerState = PlayerState.GetInstance();
        playerState.CreateHUD();
        playerState.SetCharacter(gameRoom.characters[gameRoom.chosenCharacter]);
        
        GameObject characterObj = playerState.SpawnCharacter();
        PlayerController controller = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("controllers", "PlayerController")).GetComponent<PlayerController>();
        controller.GetComponent<AudioSource>().clip = gameRoom.maps[gameRoom.curMap].backgoundMusic;
        controller.GetComponent<AudioSource>().Play();
        playerState.AttachController(controller, characterObj);
        playerState.InitHUD();
        
    }

}
