using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    private GameRoom gameRoom;
    private static PlayerState playerState;

    public int score;

    private PlayerController playerController;
    public UI_Controller_BattleHUD HUD;
    private Character character;
    private PlayerState() {
        gameRoom = GameRoom.gameRoom;
    }

    public static PlayerState GetInstance()
    {
        if (playerState == null)
        {
            playerState = new PlayerState();
        }
        return playerState;
    }

    public void SetController(PlayerController playerController)
    {
        GetInstance().playerController = playerController;
    }

    public PlayerController GetController()
    {
        return playerController;
    }
    public static bool IsUnderControll(GameObject gameObject)
    {
        return gameObject == GetInstance().playerController.character;
    }

    internal void Respawn()
    {
        AttachController(GetController(), SpawnCharacter());
        playerController.enabled = true;
        InitHUD();
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    public GameObject SpawnCharacter()
    {

        List<Vector3> spawns = gameRoom.maps[gameRoom.curMap].spawnPositions;

        GameObject characterObject = PhotonNetwork.Instantiate(character.modelPrefab.name, spawns[UnityEngine.Random.Range(0, spawns.Count)], Quaternion.identity, 0, new object[] { character.characterName });

        foreach (Ability_Data ability_data in character.abilities)
        {
            characterObject.GetComponent<AbilitySystem>().GrantAbility(ability_data.CreateInstance());
        }


        return characterObject;
    }

    public void AttachController(PlayerController playerController, GameObject characterObject)
    {
        playerController.character = characterObject;
        SetController(playerController);
    }

    public void CreateHUD()
    {

        HUD = GameObject.Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "BattleHUD")).GetComponent<UI_Controller_BattleHUD>();
        /* GameObject healthBar = GameObject.Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "HealthBar")).transform.GetChild(0).GetChild(0).gameObject;
         FloatingJoystick joyStick = GameObject.Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "Canvas_LeftJoystick")).transform.GetChild(0).GetComponent<FloatingJoystick>();*/
    }
    public void InitHUD()
    {
        GameObject characterObject = playerController.character;
        AttributeSet attributeSet = characterObject.GetComponent<AttributeSet>();

        attributeSet.OnCurrentHealthChanged += (health) =>
        {
            HUD.HealthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = health / attributeSet.maxHealth;
        };

        attributeSet.OnKilled += (id1, id2) =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (id1 != -1)
                    gameRoom.AddScore(id1, id2);
            }
        };
        AbilitySystem abilitySystem = characterObject.GetComponent<AbilitySystem>();
        HUD.button_attack.GetComponent<LongPressEventTrigger>().onLongPress.AddListener(() => abilitySystem.ActionHeld(0));
        HUD.button_skill.GetComponent<LongPressEventTrigger>().onLongPress.AddListener(() => abilitySystem.ActionHeld(1));
        HUD.button_attack.GetComponent<LongPressEventTrigger>().onPressReleased.AddListener(() => abilitySystem.ActionReleased(0));
        HUD.button_skill.GetComponent<LongPressEventTrigger>().onPressReleased.AddListener(() => abilitySystem.ActionReleased(1));
        HUD.button_attack.GetComponent<LongPressEventTrigger>().onShortPress.AddListener(() => abilitySystem.ActionPressed(0));
        HUD.button_skill.GetComponent<LongPressEventTrigger>().onShortPress.AddListener(() => abilitySystem.ActionPressed(1));
        //battleHUD.GetComponent<Canvas>().worldCamera = Camera.main;

        HUD.button_attack.GetComponent<Image>().sprite = abilitySystem.abilities[0].GetProtoData().icon;
        HUD.button_skill.GetComponent<Image>().sprite = abilitySystem.abilities[1].GetProtoData().icon;
        playerController.joyStick = HUD.joystick;

    }

    
}
