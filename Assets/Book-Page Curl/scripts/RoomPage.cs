using Photon.Pun;
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
    public TMP_Text text_button_ready;
    public TMP_Text text_button_cantStart;
    public TMP_Text text_button_start;
    public TMP_Text text_button_cancelReady;
    public string gameRoomName;
    public ScrollViewSlide MapSlide;

    private List<SeatController> seats;
    private GameRoom gameRoom;


    // Start is called before the first frame update
    void Start()
    {

    }

    public override void InitialOperation()
    {
        base.InitialOperation();

        gameRoom = PhotonNetwork.Instantiate(gameRoomName, Vector3.one, Quaternion.identity).GetComponent<GameRoom>();
        DontDestroyOnLoad(gameRoom);
        StartCoroutine(LoadingPage.JoinOrFail(5f, Time.time, Init, FlipBack, gameRoom.ConnectedToRoom, gameRoom.ProgressToRoom));
    }

    private void Update()
    {
        
    }

    public void Init()
    {
        gameRoom.Init();



        button_leave.onClick.AddListener(FlipBack);

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



        gameRoom.OnRoomChanged += refreshSeats;


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
        gameRoom.OnMapChanged += MapSlide.JumpTo;

        
        
        

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
        text_button_ready.enabled = false;
        text_button_cantStart.enabled = false;
        text_button_start.enabled = true;
        text_button_cancelReady.enabled = false;
        button_ready.enabled = false;
        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => PhotonNetwork.LoadLevel(gameRoom.maps[gameRoom.curMap].sceneName));
        gameRoom.OnReady += (ready) =>
        {
            button_ready.enabled = ready;
            text_button_ready.enabled = ready;
            text_button_cantStart.enabled = !ready;
        };

        MapSlide.OnValueChanged += (mapNr) =>
        {
            gameRoom.ChangeMap(mapNr);
        };
    }

    private void ClientView()
    {
        text_button_ready.enabled = true;
        text_button_start.enabled = false;
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


        MapSlide.ScrollRect.horizontal = false;
    }

    private void refreshSeats(RoomProperty roomProperty)
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



}