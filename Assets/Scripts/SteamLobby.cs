using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private LobbySettings settings = null;
    private NetworkManager networkManager;
    protected Callback<LobbyCreated_t> lobbyCreated;
    private const string HostAddressKey = "HostAddress";
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinReq;
    protected Callback<LobbyEnter_t> lobbyEntered;
    public string lobbyName;
    public static CSteamID LobbyId { get; private set; }



    private void Start()
    {
        networkManager = GetComponent<Manager>();

        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam Not Initialized");
            return;
        }


        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinReq = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoined);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }
    public void HostLobby(int maxplayer)
    {
        settings.OnHostLobby(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxplayer);
    }
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            settings.OnHostLobby(true);
            return;
        }
        networkManager.StartHost();
        LobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(LobbyId,"name",lobbyName);
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());

    }
    private void OnLobbyJoined(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        Debug.Log(SteamMatchmaking.GetLobbyData(callback.m_steamIDLobby, "name"));
        Debug.Log("Lobby Joined");
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        Debug.Log("Lobby Entered");

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        settings.AllClose();

        
        Debug.Log(SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name"));

    }
}
