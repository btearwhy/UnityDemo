using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller_Scoreboard : MonoBehaviour
{
    public GameObject content;
    public string itemName;
    public Button button_close;
    private List<PlayerLocal> scoreRank;
    // Start is called before the first frame update
    private void Awake()
    {
        scoreRank = new List<PlayerLocal>(); 
    }

    void Start()
    {
        button_close.onClick.AddListener(() => {
            gameObject.SetActive(false);
            PlayerController pc = PlayerState.GetInstance().GetController();
            if (pc)
            {
                pc.enabled = true;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddItem(PlayerLocal player)
    {
        GameObject item = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", itemName));
        item.transform.SetParent(content.transform);
        scoreRank.Add(player);
        item.GetComponent<UIController_ScoreItem>().Set(scoreRank.Count, player.nickName, player.character.avator, player.score);
    }

    internal void UpdateScore(List<PlayerLocal> players)
    {
        int i = 0;
        int len = scoreRank.Count;
        while(i < players.Count)
        {
            if(i < len)
            {
                content.transform.GetChild(i).GetComponent<UIController_ScoreItem>().Set(i + 1, players[i].nickName, players[i].character.avator, players[i].score);
                scoreRank[i] = players[i];
            }
            else
            {
                AddItem(players[i]);
            }
            i++;
        }
        while(i < len)
        {
            scoreRank.RemoveAt(i);
            Destroy(transform.GetChild(i));
            len--;
        }
    }
}
