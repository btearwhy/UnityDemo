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

public class UI_Controller_GameRoom : MonoBehaviour
{
    public GameRoom gameRoom;
    public TMP_Text text_roomTitle;
    public TMP_Text text_players;
    public GameObject go_seats_scrollView_content;
    public GameObject prefab_seat;
    public TMP_Dropdown dropdown_maps;
    public Image img_map;
    public Button button_leave;
    public Button button_ready;
    public TMP_Text text_button_ready;

    private string chosenMap;
    private List<GameObject> seats;

    // Start is called before the first frame update
    void Start()
    {
        gameRoom.OnInit += GUIInit;

    }

    public void GUIInit()
    {
        button_leave.onClick.AddListener(() => PhotonNetwork.LeaveRoom());

        seats = new List<GameObject>();
        for (int i = 0; i < gameRoom.maxPlayers; i++)
        {
            GameObject item = Instantiate(prefab_seat, Vector3.zero, Quaternion.identity, go_seats_scrollView_content.transform);
            item.transform.SetParent(go_seats_scrollView_content.transform);
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
            Button button = item.GetComponentInChildren<Button>();
            button.GetComponentInChildren<TMP_Text>().text = string.Empty;
            int t = i;
            button.onClick.AddListener(() => gameRoom.ChangeSeat(t));

            seats.Add(item);
        }
        
        gameRoom.OnRoomChanged += refreshSeats;
        
        img_map.sprite = gameRoom.maps[gameRoom.curMap].image;
        dropdown_maps.ClearOptions();
        IEnumerable <TMP_Dropdown.OptionData> mapOptions = from map in gameRoom.maps
                                      select new TMP_Dropdown.OptionData(map.mapName, map.image);
        dropdown_maps.AddOptions(mapOptions.ToList());
        dropdown_maps.onValueChanged.AddListener(gameRoom.ChangeMap);
        gameRoom.OnMapChanged += () => {
            Debug.Log(gameRoom.curMap);
            img_map.sprite = gameRoom.maps[gameRoom.curMap].image;
            dropdown_maps.value = gameRoom.curMap;
        };

        gameRoom.OnMasterAcquired += MasterView;
        if (PhotonNetwork.IsMasterClient)
        {
            MasterView();
        }
        else
        {
            ClientView();
            
        }
    }

    private void MasterView()
    {
        dropdown_maps.enabled = true;
        text_button_ready.text = "start";
        button_ready.enabled = false;
        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => PhotonNetwork.LoadLevel(gameRoom.maps[gameRoom.curMap].mapName));
        gameRoom.OnReady += (ready) => button_ready.enabled = ready;
    }

    private void ClientView()
    {
        text_button_ready.text = "ready";
        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => gameRoom.Ready(!gameRoom.IsReady()));

        dropdown_maps.enabled = false;
    }

    private void refreshSeats(RoomProperty roomProperty)
    {
        for (int i = 0; i < seats.Count; i++)
        {
            Button button = seats[i].GetComponentInChildren<Button>();
            ColorBlock cb = button.colors;
            if(roomProperty.seat2id[i] != null)
            {
                seats[i].GetComponentInChildren<TMP_Text>().text = (string)roomProperty.id2name[roomProperty.seat2id[i]];
                button.enabled = false;
                if ((bool)roomProperty.id2ready[roomProperty.seat2id[i]])
                {
                    cb.normalColor = Color.green;
                }
                else
                {
                    cb.normalColor = Color.yellow;
                }
                button.colors = cb;
            }
            else
            {
                seats[i].GetComponentInChildren<TMP_Text>().text = string.Empty;
                cb.normalColor = Color.white;
                button.colors = cb;
                button.enabled = true;
            }
        }
    }
}
