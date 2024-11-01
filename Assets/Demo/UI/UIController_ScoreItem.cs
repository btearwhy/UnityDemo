using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController_ScoreItem : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text textRank;
    public TMP_Text textNickName;
    public Image imageCharacterIcon;
    public TMP_Text textScore;

    public void Set(int rank, string nickName, Sprite icon, int score)
    {
        textRank.text = rank.ToString();
        textNickName.text = nickName;
        imageCharacterIcon.sprite = icon;
        textScore.text = score.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
