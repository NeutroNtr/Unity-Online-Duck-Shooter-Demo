using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class MainSpawner : NetworkBehaviour
{
    public ulong steamId;
    public GameObject LobbyPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (!authority) { return; }
        
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            SpawnLobbyPlayer();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Command]
    public void SpawnLobbyPlayer()
    {
        GameObject player = Instantiate(LobbyPlayer);
        player.GetComponent<PlayerLobby>().SteamId = steamId;
        NetworkServer.Spawn(player,connectionToClient);
        
    }
}
