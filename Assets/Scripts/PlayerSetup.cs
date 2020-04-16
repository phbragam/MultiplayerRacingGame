using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    public TextMeshProUGUI PlayerNameText;

    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            if (photonView.IsMine)
            {
                // enable car movement script and camera
                GetComponent<CarMovement>().enabled = true;
                GetComponent<LapController>().enabled = true;
                PlayerCamera.enabled = true;
            }
            else
            {
                // player is remote. Disable carMovement script and camera
                GetComponent<CarMovement>().enabled = false;
                GetComponent<LapController>().enabled = false;
                PlayerCamera.enabled = false;
            }
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {
            if (photonView.IsMine)
            {
                // enable car movement script and camera
                GetComponent<CarMovement>().enabled = true;
                GetComponent<CarMovement>().ControlsEnabled = true;
                //obs.: the order in spector matters, we need to put CarMovement over PlayerSetup (in car's prefabs), so, the Start method from CarMovement will be called first
                Debug.Log("Controls enabled");
                PlayerCamera.enabled = true;
            }
            else
            {
                // player is remote. Disable carMovement script and camera
                GetComponent<CarMovement>().enabled = false;
                PlayerCamera.enabled = false;
            }
        }
        SetPlayerUI();
    }

   private void SetPlayerUI()
    {
        if (PlayerNameText != null)
        {
            PlayerNameText.text = photonView.Owner.NickName;

            if (photonView.IsMine)
            {
                PlayerNameText.gameObject.SetActive(false);
            }
        }
    }
}
