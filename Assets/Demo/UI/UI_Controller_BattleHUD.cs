using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller_BattleHUD : MonoBehaviour
{
    public Button button_back;
    public Button button_score;
    public Button button_setting;

    public string canvas_score;
    public GameObject scoreboard;

    public string item_score;
    RoomProperty roomProperty = null;
    GameRoom gameRoom;
    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;
        scoreboard = Instantiate<GameObject>(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", canvas_score));
        
        scoreboard.SetActive(false);

        roomProperty = gameRoom.GetRoomProperty();
        foreach(var entry in roomProperty.playersMap)
        {
            scoreboard.GetComponent<UI_Controller_Scoreboard>().AddItem(entry.Value.nickName, entry.Value.score);
        }

        gameRoom.OnScoreChanged += (newRoomProperty) =>
        {
            
        };
        button_score.onClick.AddListener(() =>
        {
            scoreboard.SetActive(!scoreboard.activeSelf);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
