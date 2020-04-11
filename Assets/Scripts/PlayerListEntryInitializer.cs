using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private bool isPlayerReady = false;

    public void Initialize(int playerID, string playerName)
    {
        PlayerNameText.text = playerName;
        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            // I'am not the local player
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            // I'am the local player
            // Adding a custom property that says if player is ready or not
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { MultiplayerRacingGame.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                // Adding a new custom property that says if player is ready or not
                ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { MultiplayerRacingGame.PLAYER_READY, isPlayerReady} };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
            });
        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyImage.enabled = playerReady;

        if(playerReady == true)
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        }
        else
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
        }
    }
}
