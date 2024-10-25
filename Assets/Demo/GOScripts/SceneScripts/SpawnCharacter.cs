using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    private GameRoom gameRoom;


    // Start is called before the first frame update
    void Start()
    {
        gameRoom = GameRoom.gameRoom;        
        GameObject controller = Instantiate(AssetBundleManager.GetInstance().LoadAsset<GameObject>("controllers", "PlayerController"));
        List<Vector3> spawns = gameRoom.maps[gameRoom.curMap].spawnPositions;
        Character character = gameRoom.characters[gameRoom.chosenCharacter];
        string[] data = new string[character.abilities.Count];
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = character.abilities[i];
        }
        controller.GetComponent<PlayerController>().character = PhotonNetwork.Instantiate(character.modelPrefab.name, spawns[Random.Range(0, spawns.Count)], Quaternion.identity, 0, data);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
