using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;
public class MainSpawner : NetworkBehaviour
{
    public GameObject LobbyPlayer;
    // Start is called before the first frame update
    void Start()
    {
       
        if (!authority) { return; }
       
        
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnLobbyPlayer(NetworkConnectionToClient conn,ulong SteamId)
    {
        if (!isServer)
            return;


        if (SceneManager.GetActiveScene().name == "Lobby")
        {

            GameObject player = Instantiate(LobbyPlayer, transform.position, transform.rotation);
            gameObject.name = SteamId.ToString();

            player.GetComponent<PlayerLobby>().SteamId = SteamId;

            NetworkServer.AddPlayerForConnection(conn, player);
        }


       /*if (SceneManager.GetActiveScene().name == "Lobby")
        {

            GameObject player = Instantiate(LobbyPlayer, transform.position, transform.rotation);
            player.GetComponent<PlayerLobby>().SteamId = steamId;

            NetworkServer.AddPlayerForConnection(conn, player);
        }*/
    }


}
