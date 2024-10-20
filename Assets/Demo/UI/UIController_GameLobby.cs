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


    public GameLobby gameLobby;
    // Start is called before the first frame update
    void Start()
    {
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically

        gameLobby.OnJoinedLobby_Custom += Init;
        
    }

    //temp， should not be updated every frame
    public void Update()
    {
        text_status.text = "Status: " + PhotonNetwork.NetworkClientState;
/*        if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            overall_Canvas.enabled = true;
        }*/
    }

    public void Init()
    {

        inputField_playerName.text = "player1" ;
        inputField_roomNumber.text = "room1";
        text_title.text = "Lobby";

        button_createRoom.onClick.AddListener(() =>
        {
            string roomName = inputField_roomNumber.text;
            string playerName = inputField_playerName.text;
            if (roomName != "" && playerName != "")
            {
                gameLobby.createRoom(roomName, playerName);
            }

        });

        Refresh();
        button_refresh.onClick.AddListener(Refresh);
    }

    public void Refresh()
    {
        gameLobby.JoinLobby();
        for (int i = 0; i < viewScrollContent.transform.childCount; i++)
        {
            Destroy(viewScrollContent.transform.GetChild(i).gameObject);
        }
        if (gameLobby.createdRooms.Count == 0)
        {
            text_noRoomSign.text = "No Rooms were created yet...";
            text_noRoomSign.gameObject.SetActive(true);
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
                item.GetComponent<RectTransform>().SetParent(viewScrollContent.transform);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
                Button button_joinRoom = item.GetComponentInChildren<Button>();
                int t = i;
                button_joinRoom.onClick.AddListener(() =>
                {
                    gameLobby.JoinRoom(inputField_playerName.text, gameLobby.createdRooms[t].Name);
                });
            }
            text_noRoomSign.gameObject.SetActive(false);
        }
    }
}
