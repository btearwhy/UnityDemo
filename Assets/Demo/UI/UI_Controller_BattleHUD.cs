﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller_BattleHUD : MonoBehaviour
{
    public Canvas HUD;
    public Button button_back;
    public Button button_score;
    public Button button_setting;

    public string canvas_score;
    public string canvas_setting;
    GameObject scoreboard;
    GameObject setting;
    RoomProperty roomProperty = null;
    
    GameRoom gameRoom;
    // Start is called before the first frame update
    void Start()
    {
        HUD.worldCamera = Camera.main;
        gameRoom = GameRoom.gameRoom;

        scoreboard = Instantiate<GameObject>(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", canvas_score));
        scoreboard.SetActive(false);

        setting = Instantiate<GameObject>(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", canvas_setting));
        setting.SetActive(false);

        roomProperty = gameRoom.GetRoomProperty();
        foreach(var entry in roomProperty.playersMap)
        {
            scoreboard.GetComponent<UI_Controller_Scoreboard>().AddItem(entry.Value);
        }

        gameRoom.OnScoreChanged += (newRoomProperty) =>
        {
            roomProperty = newRoomProperty;
            List<PlayerLocal> players = new List<PlayerLocal>();
            players.AddRange(roomProperty.playersMap.Values);
            players.Sort((p1, p2) =>
            {
                return p1.score - p2.score;
            });

            scoreboard.GetComponent<UI_Controller_Scoreboard>().UpdateScore(players);
        };
        button_back.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveRoom();
        });
        button_score.onClick.AddListener(() => scoreboard.SetActive(!scoreboard.activeSelf));
        button_setting.onClick.AddListener(() => setting.SetActive(!setting.activeSelf));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}