using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController_GameLobby : MonoBehaviourPunCallbacks
{
    public Canvas overall_Canvas;
    public TMP_Text text_title;
    public TMP_Text text_status;
    public TMP_InputField inputField_roomNumber;
    public Button button_createRoom;
    public TMP_Text text_noRoomSign;
    public GameObject viewScrollContent;
    public GameObject viewScrollContentPrefab;
    public TMP_Text text_playerName;
    public TMP_InputField inputField_playerName;
    public Button button_refresh;
    public string levelName;
    //Our player name
    string playerName = "Player 1";
    //Users are separated from each other by gameversion (which allows you to make breaking changes).
    string gameVersion = "0.9";
    //The list of created rooms
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    //Use this name when creating a Room
    string roomName = "Room 1";
    Vector2 roomListScroll = Vector2.zero;
    bool joiningRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
        }
        inputField_playerName.text = playerName;
        inputField_roomNumber.text = roomName;
        text_title.text = "Lobby";
        
        button_createRoom.onClick.AddListener(() =>
        {
            string text = inputField_roomNumber.GetComponent<TMP_InputField>().text;
            if (text != "")
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsOpen = true;
                roomOptions.IsVisible = true;
                roomOptions.MaxPlayers = (byte)10; //Set any number

                PhotonNetwork.JoinOrCreateRoom(text, roomOptions, TypedLobby.Default);
            }

        });

        Refresh();
        button_refresh.onClick.AddListener(Refresh);

        overall_Canvas.enabled = false ;
    }

    //temp， should not be updated every frame
    public void Update()
    {
        text_status.text = "Status: " + PhotonNetwork.NetworkClientState;
        if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            overall_Canvas.enabled = true;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        //After we connected to Master server, join the Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list");
        //After this callback, update the room list
        createdRooms = roomList;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
        joiningRoom = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        joiningRoom = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
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

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.LoadLevel(levelName);
    }

    public void Refresh()
    {
        if (PhotonNetwork.IsConnected)
        {
            //Re-join Lobby to get the latest Room list
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        else
        {
            //We are not connected, estabilish a new connection
            PhotonNetwork.ConnectUsingSettings();
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        if (createdRooms.Count == 0)
        {
            text_noRoomSign.text = "No Rooms were created yet...";
            text_noRoomSign.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < createdRooms.Count; i++)
            {
                GameObject item = Instantiate(viewScrollContentPrefab);
                item.GetComponent<UIController_RoomList_Item>().setItem(createdRooms[i].Name, createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers, createdRooms[i].PlayerCount != createdRooms[i].MaxPlayers);
                item.GetComponent<RectTransform>().SetParent(viewScrollContent.transform);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
                Button button_joinRoom = item.GetComponentInChildren<Button>();
                int t = i;
                button_joinRoom.onClick.AddListener(() =>
                {
                    joiningRoom = true;
                    playerName = inputField_playerName.text;
                    PhotonNetwork.NickName = playerName;
                    PhotonNetwork.JoinRoom(createdRooms[t].Name);
                });
            }
            text_noRoomSign.gameObject.SetActive(false);
        }
    }
}
