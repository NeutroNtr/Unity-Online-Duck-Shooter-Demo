using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : NetworkManager
{

    public List<GameObject> Players = new List<GameObject>();
    public GameObject LobbyPlayerPrefab; // Karakter prefabinizin atandýðý deðiþken

   /* public override void OnStartServer()
    {
        Debug.Log("Server Started");
        NetworkServer.RegisterHandler<CharacterCreatorMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        Debug.Log("client Started");

        CharacterCreatorMessage characterCreatorMessage = new CharacterCreatorMessage
        {
            name = "Pepe"
        };

        NetworkClient.Send(characterCreatorMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CharacterCreatorMessage message)
    {
      
        // Geçerli sahne "Main Menu" ise "Lobby" sahnesine geçiþ yapýn
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            ServerChangeScene("Lobby");Debug.Log("çalýþ");
        }

        // Geçerli sahne "Lobby" ise oyuncuyu oluþturun ve sahneye ekleyin
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (LobbyPlayerPrefab == null)
            {
                Debug.LogError("LobbyPlayerPrefab is not assigned in the Manager.");
                return;
            }

            // Oyuncu objesini oluþturun ve sahneye ekleyin
            GameObject player = Instantiate(LobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
            
            // SteamID'yi alýp oyuncuya atayýn
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);
            var playerName = player.GetComponent<PlayerLobby>();
            if (playerName != null)
            {
                playerName.SteamIdChange(steamID.m_SteamID);
                playerName.HasAut(conn);
            }
            else
            {
                Debug.LogError("PlayerLobby component is missing from the player prefab.");
            }

            // Oyuncu objesini Players listesine ekleyin
            Players.Add(player);
            Debug.Log("Player added: " + player);
            Debug.Log("Total players: " + Players.Count);
        }
    }
   */

    


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Geçerli sahne "Main Menu" ise "Lobby" sahnesine geçiþ yapýn
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            ServerChangeScene("Lobby");
        }
        
            

       

        
       

            // Oyuncu objesini oluþturun ve sahneye ekleyin
            GameObject player = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);

            // SteamID'yi alýp oyuncuya atayýn
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);
            var playerName = player.GetComponent<MainSpawner>();
            if (playerName != null)
            {
                playerName.steamId = steamID.m_SteamID;
            }
            else
            {
                Debug.LogError("PlayerLobby component is missing from the player prefab.");
            }

            // Oyuncu objesini Players listesine ekleyin
            Players.Add(player);
            Debug.Log("Player added: " + player);
            Debug.Log("Total players: " + Players.Count);
        
    }

   
   

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connected.");
        
    }
    
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("Server scene changed: " + sceneName);
    }
   
}
/*public struct CharacterCreatorMessage : NetworkMessage
{
    public string name;
}*/



