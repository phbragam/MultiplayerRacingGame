using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            // enable car movement script and camera
            GetComponent<CarMovement>().enabled = true;
            PlayerCamera.enabled = true;
        }
        else
        {
            // player is remote. Disable carMovement script and camera
            GetComponent<CarMovement>().enabled = false;
            PlayerCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
