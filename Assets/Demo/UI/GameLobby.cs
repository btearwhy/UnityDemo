using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobby : MonoBehaviourPunCallbacks
{


    //Users are separated from each other by gameversion (which allows you to make breaking changes).
    public string gameVersion = "0.9";
    //The list of created rooms
    public List<RoomInfo> createdRooms = new List<RoomInfo>();
    //Use this name when creating a Room
    public Vector2 roomListScroll = Vector2.zero;

    public float ProgressToLobby()
    {
        float progress = 0.0f;
        if(PhotonNetwork.NetworkClientState == ClientState.ConnectingToNameServer)
        {
            progress = 0.2f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.Authenticating)
        {
            progress = 0.4f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer)
        {
            progress = 0.6f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.JoiningLobby)
        {
            progress = 0.8f;
        }
        else if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            return 1.0f;
        }
        return progress;
    }

    public bool joiningRoom = false;

    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TryConnectToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {

            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {

        char t = 'z'; 
        PhotonPeer.RegisterType(typeof(EffectContainer), (byte)t--, Serializer.Serialize<EffectContainer>, Serializer.Deserialize<EffectContainer>);
        PhotonPeer.RegisterType(typeof(Effect), (byte)t--, Serializer.Serialize<Effect>, Serializer.Deserialize<Effect>);
        PhotonPeer.RegisterType(typeof(Effect_Attack), (byte)t--, Serializer.Serialize<Effect_Attack>, Serializer.Deserialize<Effect_Attack>);
        PhotonPeer.RegisterType(typeof(Effect_AttachAttack), (byte)t--, Serializer.Serialize<Effect_AttachAttack>, Serializer.Deserialize<Effect_AttachAttack>);

        PhotonPeer.RegisterType(typeof(Effect_ChangeAttackAbility), (byte)t--, Serializer.Serialize<Effect_ChangeAttackAbility>, Serializer.Deserialize<Effect_ChangeAttackAbility>);
        PhotonPeer.RegisterType(typeof(Effect_Duration), (byte)t--, Serializer.Serialize<Effect_Duration>, Serializer.Deserialize<Effect_Duration>);
        PhotonPeer.RegisterType(typeof(Effect_Duration_AttributeChangePercentage), (byte)t--, Serializer.Serialize<Effect_Duration_AttributeChangePercentage>, Serializer.Deserialize<Effect_Duration_AttributeChangePercentage>);
        PhotonPeer.RegisterType(typeof(Effect_Instant), (byte)t--, Serializer.Serialize<Effect_Instant>, Serializer.Deserialize<Effect_Instant>);




        PhotonPeer.RegisterType(typeof(Ability), (byte)t--, Serializer.Serialize<Ability>, Serializer.Deserialize<Ability>);
        PhotonPeer.RegisterType(typeof(Ability_Attack), (byte)t--, Serializer.Serialize<Ability_Attack>, Serializer.Deserialize<Ability_Attack>);
        PhotonPeer.RegisterType(typeof(Ability_Absorb), (byte)t--, Serializer.Serialize<Ability_Absorb>, Serializer.Deserialize<Ability_Absorb>);

    }

    public IEnumerator JoinLobbyCoroutine()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        createdRooms = roomList;
    }

    internal void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            StartCoroutine(JoinLobbyCoroutine());
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        joiningRoom = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        joiningRoom = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        joiningRoom = false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        /*        //Set our player name
                PhotonNetwork.NickName = playerName;
                //Load the Scene called GameLevel (Make sure it's added to build settings)
                PhotonNetwork.LoadLevel("GameLevel");*/
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        if (!PhotonNetwork.IsConnected)
        {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
        }



    }

    public void createRoom(string roomName, string playerName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)10; 
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public void JoinRoom(string playerName, string roomName)
    {
        joiningRoom = true;
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.JoinRoom(roomName);
    }

    public bool ConnectedToLobby()
    {
        return PhotonNetwork.InLobby;
    }
}
