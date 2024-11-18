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
    public Dictionary<int, PlayerLocal> playersMap;
    public int map;
    public int maxPlayersCount;

    public RoomProperty()
    {
        playersMap = new Dictionary<int, PlayerLocal>();
    }
}

public class PlayerLocal
{
    public int actorID;
    public string nickName;
    public int seatNr;
    public bool ready;
    public Character character;
    public int score;

    public PlayerController controller;

    public PlayerLocal(int actorID, string nickName, int seatNr, bool ready, Character character, int score)
    {
        this.actorID = actorID;
        this.nickName = nickName;
        this.seatNr = seatNr;
        this.ready = ready;
        this.character = character;
        this.score = score;
    }    
}

public class GameRoom : MonoBehaviourPunCallbacks
{
    public int score = 30;
    public int maxPlayers = 10;
    public List<MapType> maps = new List<MapType>();
    public int curMap;

    public int winCondition = -20;
    public float timeCondition = 60f;
    public string endingScene = "GameLobby";
    
    public List<Character> characters = new List<Character>();
    public int chosenCharacter;

    public delegate void RoomViewHandler(RoomProperty roomProperty);
    public event RoomViewHandler OnRoomChanged;

    public delegate void ReadyHandler(bool ready);
    public event ReadyHandler OnReady;

    public delegate void MapHandler(int selected);
    public event MapHandler OnMapChanged;

    public delegate void JoinRoomHandler();
    public event JoinRoomHandler OnInit;

    public delegate void AcquireMasterHandler();
    public event AcquireMasterHandler OnMasterAcquired;

    public delegate void ScoreChangeHandler(RoomProperty roomProperty);
    public event ScoreChangeHandler OnScoreChanged;

    private readonly int currentSeatNumber = -1;


    private Player[] players;

    public static GameRoom gameRoom;
    private void Awake()
    {
        gameRoom = this;
        /*DontDestroyOnLoadManager.DontDestroyOnLoad(gameObject);*/
    }
    // Start is called before the first frame update

    public void Init()
    {
        AssetBundleManager assetBundleManager = AssetBundleManager.GetInstance();
        IEnumerable<MapType> maps_t = from map in assetBundleManager.LoadAssets<ScriptableObject>("maps")
                                      select (MapType)map;
        maps.AddRange(maps_t);
        curMap = 0;

        IEnumerable<Character> characters_t = from character in assetBundleManager.LoadAssets<ScriptableObject>("characters")
                                              select (Character)character;
        characters.AddRange(characters_t);

        winCondition = 100;
        endingScene = "Main";

        players = PhotonNetwork.PlayerList;
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
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
        playerProperties.Add("score", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    IEnumerator InitWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        
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
        curMap = selected;
        photonView.RPC("ChangeMapRPC", RpcTarget.Others, selected);
    }

    [PunRPC]
    public void ChangeMapRPC(int selected)
    {
        curMap = selected;
        OnMapChanged?.Invoke(selected);
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
        PlayerController playerController = PlayerState.GetInstance().GetController();
        if (playerController != null && playerController.character != null)
        {
            PhotonNetwork.Destroy(playerController.character);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameRoom.gameObject);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        players = PhotonNetwork.PlayerList;
        if (changedProps.ContainsKey("seat"))
        {
            RoomProperty roomProperty = roomProperty = GetRoomProperty();
            OnRoomChanged?.Invoke(roomProperty);
        }
        if (changedProps.ContainsKey("ready"))
        {
            RoomProperty roomProperty = GetRoomProperty();
            OnRoomChanged?.Invoke(roomProperty);
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
            RoomProperty roomProperty = roomProperty = GetRoomProperty();
            OnRoomChanged?.Invoke(roomProperty);
        }
        if (changedProps.ContainsKey("score"))
        {
            RoomProperty roomProperty = roomProperty = GetRoomProperty();
            OnScoreChanged?.Invoke(roomProperty);
        }

    }

    public RoomProperty GetRoomProperty()
    {
        RoomProperty property = new RoomProperty();
        property.map = curMap;
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount ; i++)
        {
            PlayerLocal player = new PlayerLocal(players[i].ActorNumber, players[i].NickName, (int)players[i].CustomProperties["seat"], (bool)players[i].CustomProperties["ready"], characters[(int)players[i].CustomProperties["character"]], (int)players[i].CustomProperties["score"]);
            property.playersMap.Add(player.actorID, player);
        }
        property.maxPlayersCount = maxPlayers;
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
        RoomProperty roomProperty = GetRoomProperty();
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

    public void AddScore(int killerName, int victimName)
    {
        Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerName);
        int scoreOriginal = (int)killer.CustomProperties["score"];
        int scoreAdd = score;
        int newScore = scoreOriginal + scoreAdd;

        Hashtable playerProperties = new Hashtable
        {
            { "score", newScore }
        };
        killer.SetCustomProperties(playerProperties);
        
        if (newScore >= winCondition)
        {
            photonView.RPC("EndGame_RPC", RpcTarget.All);
        }
        /* photonView.RPC("AddScoreRPC", RpcTarget.All, actorName, score);*/
    }

    [PunRPC]
    public void EndGame_RPC()
    {
        PlayerState.GetInstance().HUD.GetComponent<UI_Controller_BattleHUD>().scoreboard.SetActive(true);
        PlayerState.GetInstance().GetController().enabled = false;
        IEnumerator WaitSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            PhotonNetwork.LoadLevel(endingScene);
            PhotonNetwork.LeaveLobby();
        }
        StartCoroutine(WaitSeconds(10f));

    }


    internal bool ConnectedToRoom()
    {
        return PhotonNetwork.InRoom;
    }

    internal float ProgressToRoom()
    {
        if(PhotonNetwork.NetworkClientState == ClientState.Joining)
        {
            return 0.3f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.ConnectingToGameServer)
        {
            return 0.5f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.Authenticating)
        {
            return 0.8f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            return 1.0f;
        }
        return 0.0f;

    }
}
