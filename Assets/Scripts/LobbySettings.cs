using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;

public class LobbySettings : MonoBehaviour
{

    public GameObject MainMenuUI,CreatLobbyUI;
    public GameObject manager;
    public TMP_InputField field_Name,field_MaxPlayers;
    private void Start()
    {
        manager = GameObject.Find("NetworkManager");

        MainMenuUI.SetActive(true);
        CreatLobbyUI.SetActive(false);
    }



    public void CreatLobby(bool gecis)
    {
        MainMenuUI.SetActive(!gecis);
        CreatLobbyUI.SetActive(gecis);
    }
    public void OnHostLobby(bool x)
    {
        CreatLobbyUI.SetActive(x);

    }
    public void HostLobby()
    {
        if(field_MaxPlayers.text == "") { return; }
        if(field_Name.text == "") { return; }
        manager.GetComponent<SteamLobby>().lobbyName = field_Name.text;
        manager.GetComponent<Manager>().maxConnections = int.Parse(field_MaxPlayers.text);
        manager.GetComponent<SteamLobby>().HostLobby(manager.GetComponent<Manager>().maxConnections);

    }
    public void AllClose()
    {
        MainMenuUI.SetActive(false);
        CreatLobbyUI.SetActive(false);
    }
 
}
