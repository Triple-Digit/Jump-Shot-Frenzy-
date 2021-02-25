using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static Launcher instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
    #endregion

    [SerializeField] TMP_InputField m_roomNameInputField;
    [SerializeField] TMP_Text m_roomNameText;
    [SerializeField] TMP_Text m_errorText;
    [SerializeField] Transform m_roomListContent;
    [SerializeField] GameObject m_roomListItemPrefab;
    [SerializeField] Transform m_playerListContent;
    [SerializeField] GameObject m_playerListItemPrefab;
    [SerializeField] GameObject m_startButton;

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("Main Menu");
        Debug.Log("In Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(m_roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(m_roomNameInputField.text);
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("Lobby");
        m_roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        
        foreach(Transform child in m_playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {            
            Instantiate(m_playerListItemPrefab, m_playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        m_startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        m_startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        m_errorText.text = "Error: Failed to create a party room!" + message;
        MenuManager.instance.OpenMenu("Error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("Loading");        
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("Main Menu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in m_roomListContent)
        {
            Destroy(transform.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) continue;
            Instantiate(m_roomListItemPrefab, m_roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(m_playerListItemPrefab, m_playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
