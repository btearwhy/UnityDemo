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
    public GameObject go_seats_scrollView_content;
    public GameObject prefab_seat;
    public TMP_Dropdown dropdown_maps;
    public TMP_Dropdown dropdown_characters;
    public Image img_map;
    public Button button_leave;
    public Button button_ready;
    public TMP_Text text_button_ready;
    public TMP_Text text_button_start;


    private List<GameObject> seats;
    private GameRoom gameRoom;


    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;
        gameRoom.OnInit += Init;

    }

    public override void InitialOperation()
    {
        base.InitialOperation();

        StartCoroutine(LoadingPage.JoinOrFail(3f, Time.time, Init, FlipBack, gameRoom.ConnectedToRoom, gameRoom.ProgressToRoom));
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }

    public void Init()
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
        dropdown_characters.ClearOptions();
        IEnumerable<TMP_Dropdown.OptionData> characterOptions = from character in gameRoom.characters
                                                                select new TMP_Dropdown.OptionData(character.characterName, character.avator);
        dropdown_characters.AddOptions(characterOptions.ToList());
        dropdown_characters.onValueChanged.AddListener(gameRoom.ChangeCharacter);


        dropdown_maps.ClearOptions();
        IEnumerable<TMP_Dropdown.OptionData> mapOptions = from map in gameRoom.maps
                                                          select new TMP_Dropdown.OptionData(map.mapName, map.image);
        dropdown_maps.AddOptions(mapOptions.ToList());
        dropdown_maps.onValueChanged.AddListener(gameRoom.ChangeMap);
        gameRoom.OnMapChanged += () => {
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
        text_button_ready.enabled = false;
        text_button_start.enabled = true;
        button_ready.enabled = false;
        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => PhotonNetwork.LoadLevel(gameRoom.maps[gameRoom.curMap].sceneName));
        gameRoom.OnReady += (ready) => button_ready.enabled = ready;
    }

    private void ClientView()
    {
        text_button_ready.enabled = true;
        text_button_start.enabled = false;
        button_ready.onClick.RemoveAllListeners();
        button_ready.onClick.AddListener(() => gameRoom.Ready(!gameRoom.IsReady()));

        dropdown_maps.enabled = false;
    }

    private void refreshSeats(RoomProperty roomProperty)
    {
        bool[] set = new bool[seats.Count];
        foreach (var entry in roomProperty.playersMap)
        {
            int seatNr = entry.Value.seatNr;
            set[seatNr] = true;
            Button button = seats[seatNr].GetComponentInChildren<Button>();
            ColorBlock cb = button.colors;
            seats[seatNr].GetComponentInChildren<TMP_Text>().text = entry.Value.nickName;
            button.image.sprite = entry.Value.character.avator;
            button.enabled = false;
            if (entry.Value.ready)
            {
                cb.normalColor = Color.green;
            }
            else
            {
                cb.normalColor = Color.yellow;
            }
            button.colors = cb;
        }
        for (int i = 0; i < seats.Count; i++)
        {
            if (!set[i])
            {
                seats[i].GetComponentInChildren<TMP_Text>().text = string.Empty;
                Button button = seats[i].GetComponentInChildren<Button>();
                ColorBlock cb = button.colors;
                cb.normalColor = Color.white;
                button.colors = cb;
                button.enabled = true;
                button.image.sprite = sprite_seat_default;
            }
        }

    }

    private void OnDestroy()
    {
        Debug.Log("ui room destroyed");
    }



}
