﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class LapController : MonoBehaviourPun
{

    private List<GameObject> LapTriggers = new List<GameObject>();

    public enum RaiseEventCode
    {
        WhoFinishedEventCode = 0
    }

    private int finishOrder = 0;
    // Start is called before the first frame update
    void Start()
    {
        //.instance is the instance of RacingModeGameManager in RacingModeGameManager script
        foreach (GameObject lapTrigger in RacingModeGameManager.instance.LapTriggers)
        {
            LapTriggers.Add(lapTrigger);
        }
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Register a nethod that is called whenever an event is received
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }


    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    // Called when an event is raised, this EventData type corresponds to the data passed  as parameters on RaiseEvent
    void OnEvent(EventData photonEvent)
    {
        // Comparing event code
        if (photonEvent.Code == (byte) RaiseEventCode.WhoFinishedEventCode)
        {
            // Data sent by an event
            object[] data = (object[])photonEvent.CustomData;

            string nicknameOfFinishedPlayer = (string)data[0];

            finishOrder = (int)data[1];

            int viewID = (int)data[2];

            Debug.Log(nicknameOfFinishedPlayer + " " + finishOrder);

            GameObject orderUITextGameObject = RacingModeGameManager.instance.FinishOrderUIGameObjects[finishOrder - 1];
            orderUITextGameObject.SetActive(true);

            if (viewID == photonView.ViewID)
            {
                // the player is actually me
                orderUITextGameObject.GetComponent<Text>().text = finishOrder + ". " + nicknameOfFinishedPlayer + " (YOU)";
                orderUITextGameObject.GetComponent<Text>().color = Color.red;
            }
            else
            {
                orderUITextGameObject.GetComponent<Text>().text = finishOrder + ". " + nicknameOfFinishedPlayer;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LapTriggers.Contains(other.gameObject))
        {
            int indexOfTrigger = LapTriggers.IndexOf(other.gameObject);
            //Debug.Log(indexOfTrigger);
            LapTriggers[indexOfTrigger].SetActive(false);

            if (other.name == "FinishTrigger")
            {
                // game is finished
                GameFinished();
            }
        }
    }

    void GameFinished()
    {
        // Debugs CameraHolder
        // Debug.Log(GetComponent<PlayerSetup>().PlayerCamera.transform.parent.name);
        // Debugs Camera
        // Debug.Log(GetComponent<PlayerSetup>().PlayerCamera.transform.name);

        // Here we are accesssing an script that is on the same game object
        // This drops the Camera from CameraHolder
        GetComponent<PlayerSetup>().PlayerCamera.transform.parent = null;
        // Deactivate CarMovement
        GetComponent<CarMovement>().enabled = false;

        finishOrder += 1;

        string nickname = photonView.Owner.NickName;
        int viewID = photonView.ViewID;
        // Event data
        object[] data = new object[] { nickname, finishOrder, viewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        //send options
        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };
       
        PhotonNetwork.RaiseEvent((byte)RaiseEventCode.WhoFinishedEventCode, data, raiseEventOptions, sendOptions);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
