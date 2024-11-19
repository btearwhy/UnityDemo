using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPage : Page
{
    public Loading LoadingPage;
    public Sprite sprite_seat_default;
    public ScrollRect seats_ScrollView;
    public GameObject prefab_seat;
    public Button button_leave;
    public Button button_ready;
    public Button button_start;
    public TMP_Text text_button_ready;
    public TMP_Text text_button_cantStart;
    public TMP_Text text_button_start;
    public TMP_Text text_button_cancelReady;
    public string gameRoomName;
    public ScrollViewSlide MapSlide;
    public RectTransform rawImageMap;
    private List<SeatController> seats;
    private GameRoom gameRoom;

    public Vector3 cameraPos;
    // Start is called before the first frame update
    void Start()
    {
        if (isClonedForViewOnly) return;


        
    }

    public override void InitialOperation()
    {
        base.InitialOperation();

        if (isClonedForViewOnly) return;
        if (LastPage is not SettingPage)
        {
           
            StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, Init, FlipBack, ConnectedToRoom, ProgressToRoom));
        }
    }
    internal bool ConnectedToRoom()
    {
        return PhotonNetwork.InRoom;
    }

    internal float ProgressToRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joining)
        {
            return 0.3f;
        }
        else if (PhotonNetwork.NetworkClientState == ClientState.ConnectingToGameServer)
        {
            return 0.5f;
        }
        else if (PhotonNetwork.NetworkClientState == ClientState.Authenticating)
        {
            return 0.8f;
        }
        else if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            return 1.0f;
        }
        return 0.0f;

    }

    private void Update()
    {
        
    }

    public void Init()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameRoom = PhotonNetwork.Instantiate(gameRoomName, Vector3.one, Quaternion.identity).GetComponent<GameRoom>();
        }
        else
        {
            gameRoom = FindObjectOfType<GameRoom>();
        }

        button_leave.onClick.RemoveAllListeners();
        button_leave.onClick.AddListener(FlipBack);

        gameRoom.OnRoomChanged += RefreshSeats;

        gameRoom.OnMapChanged += MapSlide.JumpTo;

        gameRoom.OnMasterAcquired += MasterView;

        gameRoom.OnLeave += FlipBack;

        button_start.onClick.RemoveAllListeners();
        button_start.onClick.AddListener(() => {
            gameRoom.StartGame();
            rawImageMap.gameObject.SetActive(true);

        });

        gameRoom.OnReady += (ready) =>
        {
            button_ready.enabled = ready;
            text_button_start.enabled = ready;
            text_button_cantStart.enabled = !ready;
        };

        MapSlide.ClearValueEvent();
        MapSlide.OnValueChanged += (mapNr) =>
        {
            gameRoom.ChangeMap(mapNr);
        };

        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => {
            if (gameRoom.IsReady())
            {
                text_button_ready.enabled = true;
                text_button_cancelReady.enabled = false;
            }
            else
            {
                text_button_ready.enabled = false;
                text_button_cancelReady.enabled = true;
            }
            gameRoom.Ready(!gameRoom.IsReady());
        });

        gameRoom.Init();



        

        seats = new List<SeatController>();
        for (int i = 0; i < gameRoom.maxPlayers; i++)
        {
            GameObject item = Instantiate(prefab_seat, Vector3.zero, Quaternion.identity, seats_ScrollView.content);
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
            SeatController seatController = item.GetComponent<SeatController>();
            seatController.playerName.text = string.Empty;
            int t = i;
            seatController.button.onClick.AddListener(() => gameRoom.ChangeSeat(t));

            seats.Add(seatController);
        }



        


        /* dropdown_characters.ClearOptions();
         IEnumerable<TMP_Dropdown.OptionData> characterOptions = from character in gameRoom.characters
                                                                 select new TMP_Dropdown.OptionData(character.characterName, character.avator);
         dropdown_characters.AddOptions(characterOptions.ToList());
         dropdown_characters.onValueChanged.AddListener(gameRoom.ChangeCharacter);*/


        foreach (MapType map in gameRoom.maps)
        {
            GameObject gameObject = new GameObject();
            Image image = gameObject.AddComponent<Image>();
            image.sprite = map.image;
            RectTransform rect = image.GetComponent<RectTransform>();
            gameObject.SetActive(true);
            MapSlide.AddItem(rect);
        }
        MapSlide.Init();
        

        
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
        button_start.gameObject.SetActive(true);
        button_ready.gameObject.SetActive(false);
        text_button_cantStart.enabled = false;
        text_button_start.enabled = true;
    }

    private void ClientView()
    {
        button_start.gameObject.SetActive(false);
        button_ready.gameObject.SetActive(true);
        text_button_ready.enabled = true;
        text_button_cancelReady.enabled = false;


        MapSlide.ScrollRect.horizontal = false;
    }

    private void RefreshSeats(RoomProperty roomProperty)
    {
        bool[] set = new bool[seats.Count];
        
        foreach (var entry in roomProperty.playersMap)
        {
            int seatNr = entry.Value.seatNr;
            set[seatNr] = true;
            Button button = seats[seatNr].button;
            ColorBlock cb = button.colors;
            seats[seatNr].playerName.text = entry.Value.nickName;
            seats[seatNr].image_background.sprite = entry.Value.character.avator;
            button.enabled = false;
            if (entry.Value.ready)
            {
                cb.normalColor = Color.green;
                seats[seatNr].image_ready.enabled = true;
                seats[seatNr].image_notReady.enabled = false;
            }
            else
            {
                cb.normalColor = Color.yellow;
                seats[seatNr].image_ready.enabled = false;
                seats[seatNr].image_notReady.enabled = true;
            }
            button.colors = cb;
        }
        for (int i = 0; i < seats.Count; i++)
        {
            if (!set[i])
            {
                seats[i].playerName.text = string.Empty;
                Button button = seats[i].button;
                ColorBlock cb = button.colors;
                cb.normalColor = Color.white;
                button.colors = cb; 
                button.enabled = true;
                button.image.sprite = sprite_seat_default;
                seats[i].image_background.enabled = false;
                seats[i].image_notReady.enabled = false;
                seats[i].image_ready.enabled = false;
            }
        }

    }

    private void OnDestroy()
    {
        Debug.Log("ui room destroyed");
    }

    public override void LeaveLeftTop()
    {
        base.LeaveLeftTop();
        for (int i = 0; i < seats_ScrollView.content.childCount; i++)
        {
            Destroy(seats_ScrollView.content.GetChild(i).gameObject);
        }
        MapSlide.Clear();
        PhotonNetwork.LeaveRoom();
    }


}
