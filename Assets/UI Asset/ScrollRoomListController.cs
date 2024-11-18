using DG.Tweening;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRoomListController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public RectTransform Image_Refresh;
    public ScrollRect scrollRect;
    public Tween tweenForRefresh;
    public LobbyPage page;
    public RectTransform Panel_NoRoomSign;
    public GameObject viewScrollContentPrefab;
    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        Panel_NoRoomSign.localPosition = scrollRect.content.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect.content.localPosition.y < -150)
        {
            if (!tweenForRefresh.IsActive())
            {
                tweenForRefresh = Image_Refresh.DORotate(Image_Refresh.rotation.eulerAngles + new Vector3(0, 0, -180), 1.0f).OnStart(() => Image_Refresh.gameObject.SetActive(true)).OnComplete(() =>
                {
                    Image_Refresh.gameObject.SetActive(false);
                    page.RefreshRoomList();
                });
            }
        }
        {
            
            if (!tweenForRefresh.IsPlaying())
            {
                tweenForRefresh.Play();
            }

        }

    }

    internal void Refresh(List<RoomInfo> rooms)
    {
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }
        if (rooms.Count == 0)
        {
            Panel_NoRoomSign.gameObject.SetActive(true);
        }
        else
        {
            bool anyVisible = false;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].IsVisible)
                {
                    RoomPanelController roomItem = Instantiate(viewScrollContentPrefab).GetComponent<RoomPanelController>();
                    roomItem.Text_RoomName.text = rooms[i].Name;
                    roomItem.Text_PlayerCount.text = rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers;
                    roomItem.button_join.interactable = rooms[i].IsOpen;
                    //item.GetComponent<UIController_RoomList_Item>().setItem(rooms[i].Name, rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers, rooms[i].PlayerCount != rooms[i].MaxPlayers);
                    roomItem.GetComponent<RectTransform>().SetParent(scrollRect.content);
                    roomItem.transform.localScale = Vector3.one;
                    roomItem.transform.localPosition = new Vector3(roomItem.transform.localPosition.x, roomItem.transform.localPosition.y, 0);
                    roomItem.Text_Full.enabled = false;
                    roomItem.Text_InGame.enabled = false;
                    roomItem.Text_join.enabled = false;

                    if (!rooms[i].IsOpen)
                    {
                        if (rooms[i].MaxPlayers == rooms[i].PlayerCount)
                        {
                            roomItem.Text_Full.enabled = true;
                        }
                        else
                        {
                            roomItem.Text_InGame.enabled = true;
                        }
                    }
                    else
                    {
                        string roomName = rooms[i].Name;
                        roomItem.button_join.onClick.AddListener(() =>
                        {
                            page.EnterRoom(roomName);
                        });
                        roomItem.Text_join.enabled = true;
                    }
                }
            }
            if (anyVisible)
            {
                Panel_NoRoomSign.gameObject.SetActive(true);
            }
            else
            {
                Panel_NoRoomSign.gameObject.SetActive(false);
            }
        }
    }
}
