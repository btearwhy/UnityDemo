using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System.Linq;
using System;

public class RoomProperty
{
    public string map { get; set; }
    public Hashtable seat2id { get; set; }
    public Hashtable id2name { get; set; }
    public Hashtable id2ready { get; set; }   
    public Hashtable id2character { get; set; }

    public RoomProperty()
    {
        map = string.Empty;
        seat2id = new Hashtable();
        id2name = new Hashtable();
        id2ready = new Hashtable();
        id2character = new Hashtable();
    }
}



public class GameRoom : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    public List<MapType> maps;
    public int curMap;

    public List<Character> characters;
    public int chosenCharacter;

    public delegate void RoomViewHandler(RoomProperty roomProperty);
    public event RoomViewHandler OnRoomChanged;

    public delegate void ReadyHandler(bool ready);
    public event ReadyHandler OnReady;

    public delegate void MapHandler();
    public event MapHandler OnMapChanged;

    public delegate void JoinRoomHandler();
    public event JoinRoomHandler OnInit;

    public delegate void AcquireMasterHandler();
    public event AcquireMasterHandler OnMasterAcquired;

    private readonly int currentSeatNumber = -1;


    private Player[] players;

    public static GameRoom gameRoom;
    private void Awake()
    {
        gameRoom = this;
        DontDestroyOnLoadManager.DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start() 
    {

        AssetBundleManager assetBundleManager = AssetBundleManager.GetInstance();
        IEnumerable<MapType> maps_t = from map in assetBundleManager.LoadAssets<ScriptableObject>("maps")
                                    select (MapType)map;
        maps.AddRange(maps_t);
        curMap = 0;

        IEnumerable<Character> characters_t = from character in assetBundleManager.LoadAssets<ScriptableObject>("characters")
                                      select (Character)character;
        characters.AddRange(characters_t);

        StartCoroutine(InitWhenConnected());
       

    }

    IEnumerator InitWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        players = PhotonNetwork.PlayerList;
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        OnInit.Invoke();
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
        playerProperties.Add("character", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    internal void ChangeCharacter(int selected)
    {
        Hashtable hashtable = new Hashtable
        {
            { "character", selected }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }


    internal void ChangeMap(int selected)
    {
        photonView.RPC("ChangeMapRPC", RpcTarget.All, selected);
    }

    [PunRPC]
    public void ChangeMapRPC(int selected)
    {
        curMap = selected;
        OnMapChanged.Invoke();
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
        if (changedProps.ContainsKey("seat"))
        {
            RoomProperty roomProperty = roomProperty = SetRoomProperty();
            OnRoomChanged.Invoke(roomProperty);
        }
        if (changedProps.ContainsKey("ready"))
        {
            RoomProperty roomProperty = SetRoomProperty();
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
        if (changedProps.ContainsKey("character"))
        {
            RoomProperty roomProperty = roomProperty = SetRoomProperty();
            OnRoomChanged.Invoke(roomProperty);
        }

    }

    private RoomProperty SetRoomProperty()
    {
        RoomProperty property = new RoomProperty();
        foreach (Player player in players)
        {
            property.id2name.Add(player.ActorNumber, player.NickName);
            property.id2ready.Add(player.ActorNumber, player.CustomProperties["ready"]);
            property.seat2id.Add(player.CustomProperties["seat"], player.ActorNumber);
            property.id2character.Add(player.ActorNumber, player.CustomProperties["character"]);
        }

        return property;
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        players = PhotonNetwork.PlayerList;
        RoomProperty roomProperty = SetRoomProperty();
        OnRoomChanged.Invoke(roomProperty);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        photonView.RPC("MasterRPC", newMasterClient);
    }

    [PunRPC]
    public void MasterRPC()
    {
        Ready(true);
        OnMasterAcquired.Invoke();
    }
}
