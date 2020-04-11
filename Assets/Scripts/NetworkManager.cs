using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string GameMode;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;
    public Text RoomInfoText;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListContent;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;

    private Dictionary<int, GameObject> playerListGameObjects;
    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(LoginUIPanel.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region UI Callbacks Methods

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel.name);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        ActivatePanel(CreatingRoomInfoUIPanel.name);
        if (GameMode != null)
        {
            string roomName = RoomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;

            string[] roomPropsInLobby = { "gm" }; //gm = game mode

            // two game modes
            // 1. racing = "rc"
            // 2. death race = "dr"

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public void OnJoinRandomRommButtonClicked(string _gameMode)
    {
        GameMode = _gameMode;

        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", _gameMode } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnBackButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        ActivatePanel(GameOptionsUIPanel.name);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
    }

    // Called when this client created a room and entered it. OnJoinedRoom() will be called as well
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName 
            + " joined to " 
            + PhotonNetwork.CurrentRoom.Name 
            + " / Player count: "
            + PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(InsideRoomUIPanel.name);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            RoomInfoText.text = "Room name: " 
                + PhotonNetwork.CurrentRoom.Name
                + " "
                + " Players/Max.Players: "
                + PhotonNetwork.CurrentRoom.PlayerCount
                + " / "
                + PhotonNetwork.CurrentRoom.MaxPlayers;

            // This is because we dont need to INSTANTIATE this list again when leave room and jointo another
            if(playerListGameObjects == null)
            {
                playerListGameObjects = new Dictionary<int, GameObject>();
            }
            

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject playerListGameObject = Instantiate(PlayerListPrefab);
                playerListGameObject.transform.SetParent(PlayerListContent.transform);
                playerListGameObject.transform.localScale = Vector3.one;
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(player.ActorNumber, player.NickName);

                playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        // if there is no room, create one
        if (GameMode != null)
        {
            string roomName = RoomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;

            string[] roomPropsInLobby = { "gm" }; //gm = game mode

            // two game modes
            // 1. racing = "rc"
            // 2. death race = "dr"

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomInfoText.text = "Room name: "
               + PhotonNetwork.CurrentRoom.Name
               + " "
               + " Players/Max.Players: "
               + PhotonNetwork.CurrentRoom.PlayerCount
               + " / "
               + PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = Instantiate(PlayerListPrefab);
        playerListGameObject.transform.SetParent(PlayerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
        playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomInfoText.text = "Room name: "
               + PhotonNetwork.CurrentRoom.Name
               + " "
               + " Players/Max.Players: "
               + PhotonNetwork.CurrentRoom.PlayerCount
               + " / "
               + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Destroying game object from player who left in playerListGameObject
        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptionsUIPanel.name);

        // Cleaning all the list because you left the room
        foreach (GameObject playerListGameObject in playerListGameObjects.Values)
        {
            Destroy(playerListGameObject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    #endregion

    #region Public Methods

    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
        InsideRoomUIPanel.SetActive(InsideRoomUIPanel.name.Equals(panelNameToBeActivated));
    }

    public void SetGameMode(string _gameMode)
    {
        GameMode = _gameMode;
    }

    #endregion
}
