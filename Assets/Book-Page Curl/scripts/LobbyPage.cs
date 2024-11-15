using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPage : Page
{
    public Loading LoadingPage;
    private GameLobby gameLobby;


    public TMP_InputField inputField_roomNumber;
    public Button button_createRoom;
    public Button button_back;
    public RectTransform panel_noRoomSign;
    public RectTransform viewScrollContent;
    public GameObject viewScrollContentPrefab;


    private string playername = "test";

    public override void InitialOperation()
    {
        base.InitialOperation();

        gameLobby = GetComponent<GameLobby>();

        gameLobby.ConnectToLobby();

        StartCoroutine(LoadingPage.JoinOrFail(3f, Time.time, Init, FlipBack, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby));
    }

    private void Start()
    {
        button_back.GetComponent<ButtonController>().OnComplete += FlipBack;
    }

    //temp， should not be updated every frame
    public void Update()
    {

    }

    public void Init()
    {
        button_createRoom.onClick.AddListener(() =>
        {
            string roomName = inputField_roomNumber.text;
            string playerName = this.playername;
            if (roomName != "" && playerName != "")
            {
                gameLobby.createRoom(roomName, playerName);
                TurnPage.AutoFlip(FlipRegion.RightBottom);
            }

        });

        Refresh();
        //button_refresh.onClick.AddListener(gameLobby.JoinLobby);
    }

    public void Refresh()
    {

        for (int i = 0; i < viewScrollContent.childCount; i++)
        {
            Destroy(viewScrollContent.GetChild(i).gameObject);
        }
        if (gameLobby.createdRooms.Count == 0)
        {
            panel_noRoomSign.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < gameLobby.createdRooms.Count; i++)
            {
                GameObject item = Instantiate(viewScrollContentPrefab);
                item.transform.GetChild(0).GetComponent<TMP_Text>().text = gameLobby.createdRooms[i].Name;
                item.transform.GetChild(1).GetComponent<TMP_Text>().text = gameLobby.createdRooms[i].PlayerCount + "/" + gameLobby.createdRooms[i].MaxPlayers;
                item.transform.GetChild(2).GetComponent<Button>().interactable = gameLobby.createdRooms[i].PlayerCount != gameLobby.createdRooms[i].MaxPlayers;
                //item.GetComponent<UIController_RoomList_Item>().setItem(gameLobby.createdRooms[i].Name, gameLobby.createdRooms[i].PlayerCount + "/" + gameLobby.createdRooms[i].MaxPlayers, gameLobby.createdRooms[i].PlayerCount != gameLobby.createdRooms[i].MaxPlayers);
                item.GetComponent<RectTransform>().SetParent(viewScrollContent);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
                Button button_joinRoom = item.GetComponentInChildren<Button>();
                int t = i;
                button_joinRoom.onClick.AddListener(() =>
                {
                    gameLobby.JoinRoom(playername, gameLobby.createdRooms[t].Name);
                    TurnPage.AutoFlip(FlipRegion.RightBottom);
                });
            }
            panel_noRoomSign.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void LeaveLeftTop()
    {
        base.LeaveLeftTop();

        Quit();
    }

}
