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
    public TMP_Text text_time;
    public GameObject HealthBar;
    public GameObject SkillUIPlaceHolder;
    public Button button_attack;
    public Button button_skill;
    public string canvas_score;
    public string canvas_setting;
    public GameObject scoreboard;
    public GameObject setting;
    RoomProperty roomProperty = null;
    
    GameRoom gameRoom;
    // Start is called before the first frame update
    private void Awake()
    {

    }

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
                return p2.score - p1.score;
            });

            scoreboard.GetComponent<UI_Controller_Scoreboard>().UpdateScore(players);
        };
        button_back.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveRoom();
        });
        button_score.onClick.AddListener(() =>
        {
            scoreboard.SetActive(!scoreboard.activeSelf);
        });
        button_setting.onClick.AddListener(() => {
            PlayerController pc = PlayerState.GetInstance().GetController();
            if (pc)
            {
                pc.enabled = !pc.enabled;
            }
            setting.SetActive(!setting.activeSelf);
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
