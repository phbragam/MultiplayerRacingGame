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

    public void Initialize(int playerID, string playerName)
    {
        PlayerNameText.text = playerName;
    }
}
