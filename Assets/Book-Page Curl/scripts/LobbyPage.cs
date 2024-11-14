using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPage : Page
{
    public Loading LoadingPage;
    public RectTransform PanelToShowWhileLoading;
    private GameLobby gameLobby;

    public override void InitialOperatiion()
    {
        base.InitialOperatiion();

        gameLobby = GetComponent<GameLobby>();

        gameLobby.ConnectToLobby();

        StartCoroutine(LoadingPage.JoinOrFail(3f, Time.time, () => { }, Back, gameLobby.ConnectedToLobby, gameLobby.ProgressToLobby)) ;
    }


}
