
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
    public ScrollRoomListController scrollRoom;



    private string playername = "player";

    
    public override void InitialOperation()
    {
        base.InitialOperation();

        if(LastPage is not SettingPage)
        {
            gameLobby.TryConnectToMaster();
            StartCoroutine(gameLobby.JoinLobbyCoroutine());
            StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, Init, FlipBack, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby));
        }
     
      /*  if (LastPage is MainPage)
        {
            gameLobby.ConnectToMaster();
            StartCoroutine(gameLobby.JoinLobbyCoroutine());
            StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, Init, FlipBack, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby));
        }
        else {
            
            StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, Init, FlipBack, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby));
        }*/
    }

    private void Start()
    {
        if (isClonedForViewOnly) return;
        gameLobby = GetComponent<GameLobby>();

        button_back.GetComponent<ButtonController>().OnComplete += FlipBack;

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
    }


    public void Init()
    {
        scrollRoom.Refresh(gameLobby.createdRooms);
    }

    public void RefreshRoomList()
    {
        gameLobby.TryConnectToMaster();
        StartCoroutine(gameLobby.JoinLobbyCoroutine());
        StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, () => scrollRoom.Refresh(gameLobby.createdRooms), FlipBack, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby));
    }

    public void EnterRoom(string roonName)
    {
        gameLobby.JoinRoom(playername, roonName);
        TurnPage.AutoFlip(FlipRegion.RightBottom);
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
