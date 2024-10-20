using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System;
using System.Linq;

public class RoomProperty
{
    public string map;
    public Hashtable seat2id;
    public Hashtable id2name;
    public Hashtable id2ready;   

    public RoomProperty()
    {
        map = string.Empty;
        seat2id = new Hashtable();
        id2name = new Hashtable();
        id2ready = new Hashtable();
    }
}

public class PlayerProperty
{
    public Hashtable map;
    public bool IsReady()
    {
        return (bool)map["ready"];
    }
}
public class GameRoom : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    //地图暂时
    public string map;
    public delegate void RoomViewHandler(RoomProperty roomProperty);
    public event RoomViewHandler OnRoomChanged;

    public delegate void ReadyHandler(bool ready);
    public event ReadyHandler OnReady;

    public delegate void JoinRoomHandler();
    public event JoinRoomHandler OnJoin;
    private readonly int currentSeatNumber = -1;

    public RoomProperty roomProperty;
    public PlayerProperty playerProperty;
    private Player[] players;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitWhenConnected());
       

    }

    IEnumerator InitWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        map = "SampleScene";
        players = PhotonNetwork.PlayerList;
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        OnJoin.Invoke();
        Hashtable playerProperties = new Hashtable();

        if (PhotonNetwork.IsMasterClient)
        {
            playerProperties.Add("ready", true);

        }
        else
        {
            playerProperties.Add("ready", false);
        }
        
        playerProperties.Add("seat", GetAvailableSeatNumber());
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public int GetAvailableSeatNumber()
    {

        bool[] available = Enumerable.Repeat(true, maxPlayers).ToArray();

        foreach (Player player in players)
        {
            if (player.CustomProperties["seat"] == null) continue;
            int seat = (int)player.CustomProperties["seat"];
            
            available[seat] = false;
        }
        for(int i = 0; i < available.Length; i++)
        {
            if (available[i]) return i; 
        }
        return -1;
    }

    internal void Ready(bool ready)
    {
        Hashtable hashtable = new Hashtable
        {
            { "ready", ready }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        players = PhotonNetwork.PlayerList;
        RoomProperty roomProperty = new RoomProperty();
        foreach(Player player in players)
        {
            roomProperty.id2name.Add(player.ActorNumber, player.NickName);
            roomProperty.id2ready.Add(player.ActorNumber, player.CustomProperties["ready"]);
            roomProperty.seat2id.Add(player.CustomProperties["seat"], player.ActorNumber);
        }
        if (changedProps.ContainsKey("seat"))
        {
            OnRoomChanged.Invoke(roomProperty);
        }
        if (changedProps.ContainsKey("ready"))
        {
            OnRoomChanged.Invoke(roomProperty);
            if (PhotonNetwork.IsMasterClient)
            {
                int cnt = 0;
                foreach (Player player in players)
                {
                    if ((bool)player.CustomProperties["ready"])
                    {
                        cnt++;
                    }
                }
                OnReady.Invoke(cnt == players.Length);
            }
        }
    }

    public bool IsReady()
    {
        return (bool)PhotonNetwork.LocalPlayer.CustomProperties["ready"];
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

    }

    public void ChangeSeat(int cur)
    {
        Hashtable playerProperties = new Hashtable
        {
            { "seat", cur }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
}
