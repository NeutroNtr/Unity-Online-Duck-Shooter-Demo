using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : NetworkManager
{
    public GameObject LobbyPlayerPrefab;
   
    public List<GameObject> Players = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
            
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            Debug.Log("sad");
            ServerChangeScene("Lobby");

        }
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Debug.Log("add");
            GameObject player = Instantiate(LobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
            player.transform.parent = GameObject.Find("Case").transform;
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);
            var playerName = player.GetComponent<PlayerLobby>();
            playerName.SteamIdChange(steamID.m_SteamID);
        }
      
        Players.Add(conn.identity.gameObject);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("Sunucu sahnesi deðiþti: " + sceneName);


    }

}
