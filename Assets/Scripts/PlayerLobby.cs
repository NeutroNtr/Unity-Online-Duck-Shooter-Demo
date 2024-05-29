using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Org.BouncyCastle.Asn1.X509;
using Steamworks;
using UnityEngine.UI;

public class PlayerLobby : NetworkBehaviour
{
    #region Name
    public Manager manager;
    public TMP_Text NameText;
    private Button StartButton, ReadyButton;
    public RawImage ProfileImage;

    [SyncVar(hook = nameof(OnSteamIdChange))]
    public ulong SteamId;

    public GameObject ReadyImage;

    [SyncVar(hook = nameof(OnReady))]
    public bool Ready;

    public bool readyAllOf;
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    public void CheckReadyState()
    {
        if (!isServer) { return; }

        if (CheckAllPlayerReadys(manager.Players))
        {
            StartButton = GameObject.Find("Start").GetComponent<Button>();
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;

        }

    }
    bool CheckAllPlayerReadys(List<GameObject> list)
    {
        foreach(GameObject obj in list)
        {
            PlayerLobby player = obj.GetComponent<PlayerLobby>();
            if(player == null || !player.Ready)
            {
                return false;
            }
        }
        return true;
    }
    public override void OnStartClient()
    {
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }
    private void Start()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<Manager>();
        StartButton = GameObject.Find("Start").GetComponent<Button>();
        ReadyButton = GameObject.Find("Ready").GetComponent<Button>();
        StartButton.enabled = false;
        StartButton.gameObject.GetComponent<Image>().enabled = false;
        ReadyButton.gameObject.GetComponent<Image>().enabled = true;
        ReadyButton.enabled = true;
        StartButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ReadyButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        if (isServer)
        {
            StartButton.enabled = true;
            StartButton.gameObject.GetComponent<Image>().enabled = true;
            StartButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            StartButton.interactable = false;

        }
        ReadyButton.onClick.AddListener(ReadyFunc);

    }

    private void Update()
    {
        
    }
    public void OnReady(bool old, bool newBool)
    {
        ReadyImage.SetActive(newBool);
    }

    public void SteamIdChange(ulong name)
    {

        SteamId = name;

    }

    void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID != SteamId) { return; }

        ProfileImage.texture = GetSteamImageAsTexture(callback.m_iImage);
    }
    private void OnSteamIdChange(ulong oldText, ulong newText)
    {
        Debug.LogError(newText);
        var cSteamId = new CSteamID(newText);
        NameText.text = SteamFriends.GetFriendPersonaName(cSteamId);
        int imageId = SteamFriends.GetLargeFriendAvatar(cSteamId);
         
        if (imageId == -1) { return; }

        ProfileImage.texture = GetSteamImageAsTexture(imageId);
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint widht, out uint height);

        if (isValid)
        {
            byte[] image = new byte[widht * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image,(int)(widht * height * 4));

            if(isValid)
            {
                texture = new Texture2D((int)widht,(int)height,TextureFormat.RGBA32,false,true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        return texture;
    }
    #endregion

    public void ReadyFunc()
    {
        Ready = !Ready;
        CmdReadyButton();
    }
    [Command]
    public void CmdReadyButton()
    {
        CheckReadyState();
    }
}
