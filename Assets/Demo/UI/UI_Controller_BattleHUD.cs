using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller_BattleHUD : MonoBehaviour
{
    public Canvas HUD;
    public Button button_back;
    public Button button_score;
    public Button button_setting;
    public GameObject HealthBar;
    public GameObject SkillUIPlaceHolder;
    public Button button_attack;
    public Button button_skill;
    public RectTransform scoreboard;
    public RectTransform setting;
    RoomProperty roomProperty = null;

    public Button[] buttons;
    GameRoom gameRoom;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    void Start()
    {
        HUD.worldCamera = Camera.main;
        gameRoom = GameRoom.gameRoom;

        
        scoreboard.gameObject.SetActive(false);


        setting.gameObject.SetActive(false);
        

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
                return p2.score - p1.score;
            });

            scoreboard.GetComponent<UI_Controller_Scoreboard>().UpdateScore(players);
        };
        button_back.onClick.AddListener(() =>
        {
            GameRoom.gameRoom.Leave();
        });
        button_score.onClick.AddListener(() =>
        {
            PlayerController pc = PlayerState.GetInstance().GetController();
            if (pc)
            {
                pc.enabled = false;
            }
            scoreboard.gameObject.SetActive(true);
        });
        button_setting.onClick.AddListener(() => {
            PlayerController pc = PlayerState.GetInstance().GetController();
            if (pc)
            {
                pc.enabled = false;
            }
            setting.gameObject.SetActive(true);
        });

        buttons = GetComponentsInChildren<Button>();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
