using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattle : MonoBehaviour
{
    private GameRoom gameRoom;


    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;

        GameObject characterObject = SpawnCharacterAndController();

    }


    GameObject SpawnCharacterAndController()
    {
        GameObject controller = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("controllers", "PlayerController"));
        List<Vector3> spawns = gameRoom.maps[gameRoom.curMap].spawnPositions;
        Character character = gameRoom.characters[gameRoom.chosenCharacter];

        GameObject characterObject = PhotonNetwork.Instantiate(character.modelPrefab.name, spawns[Random.Range(0, spawns.Count)], Quaternion.identity);
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
