using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour
{
    private GameRoom gameRoom;


    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;

        GameObject characterObject = SpawnCharacterAndController();
        AttributeSet attributeSet = characterObject.GetComponent<AttributeSet>();
        GameObject healthBar =  Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "HealthBar")).transform.GetChild(0).GetChild(0).gameObject;

        attributeSet.OnCurrentHealthChanged += (health) =>
        {
            healthBar.GetComponent<Image>().fillAmount = health / attributeSet.maxHealth;
        };

        attributeSet.OnKilled += (id1, id2) => {
            if (PhotonNetwork.IsMasterClient)
            {
                gameRoom.AddScore(id1, id2);
            }    
        };

        GameObject battleHUD = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("ui", "BattleHUD"));
        battleHUD.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public GameObject SpawnCharacterAndController()
    {
        GameObject controller = Object.Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("controllers", "PlayerController"));
        List<Vector3> spawns = gameRoom.maps[gameRoom.curMap].spawnPositions;
        Character character = gameRoom.characters[gameRoom.chosenCharacter];

        GameObject characterObject = PhotonNetwork.Instantiate(character.modelPrefab.name, spawns[Random.Range(0, spawns.Count)], Quaternion.identity, 0, new object[] { character.characterName });
        controller.GetComponent<PlayerController>().character = characterObject;
        PlayerState.GetInstance().SetController(controller.GetComponent<PlayerController>());
        foreach (string abilityName in character.abilities)
        {
            /*            Ability ability = (Ability)AssetBundleManager.GetInstance().LoadAsset<ScriptableObject>("abilities", abilityName);*/
            characterObject.GetComponent<AbilitySystem>().GrantAbility(abilityName);
        }


        return characterObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
