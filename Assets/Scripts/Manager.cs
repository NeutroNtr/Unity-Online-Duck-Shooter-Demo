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
    int i;
    
    public List<GameObject> Players = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
       
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            ServerChangeScene("Lobby");

        }
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            GameObject player = Instantiate(LobbyPlayerPrefab);
            player.name = "player " + i++;
            NetworkServer.AddPlayerForConnection(conn, player);
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);
            var playerName = player.GetComponent<PlayerLobby>();
            playerName.SteamIdChange(steamID.m_SteamID);
        }
      
        Players.Add(conn.identity.gameObject);
        Debug.LogError(conn.identity.gameObject);
        Debug.LogError(Players);

    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("Sunucu sahnesi deðiþti: " + sceneName);


    }

}
