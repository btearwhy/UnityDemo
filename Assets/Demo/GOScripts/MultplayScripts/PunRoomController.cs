﻿using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class PunRoomController : MonoBehaviourPunCallbacks
{

    //Player instance prefab, must be located in the Resources folder
    public GameObject controllerPrefab;
    public GameObject playerPrefab;
    //Player spawn point
    public Transform spawnPoint;

    private GameObject character;
    GameObject ControllerObject;
    // Use this for initialization
    void Start()
    {
        //In case we started this demo with the wrong scene being active, simply load the menu scene
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }
        StartCoroutine(SpawnPlayerWhenConnected());
        ControllerObject = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity);
        /*        character = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);*/
        /*GameObject ControllerObject = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity);*/
        /*        ControllerObject.GetComponent<PlayerController>().character = character;*/
        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate\


    }

    IEnumerator SpawnPlayerWhenConnected()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);

        character = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        
        ControllerObject.GetComponent<PlayerController>().character = character;
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        //Leave this Room
        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        //Show the Room name
        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);

        //Show the list of the players connected to this Room
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
            string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        //We have left the Room, return back to the GameLobby
        PhotonNetwork.Destroy(character);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }
}