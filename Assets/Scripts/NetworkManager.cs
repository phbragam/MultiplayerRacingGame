﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;

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
    }
    #endregion
}
