using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TimeCountdownManager : MonoBehaviourPunCallbacks
{
    private Text TimeUIText;
    private float TimeToStartRace = 5f;

    private void Awake()
    {
        // RacingModeGameManager.instance is instance of RacingModeGameManeger (see RacingModeGameManeger script)
        // We are accessing another game object without dragging and dropping the game object (script component) thanks to singleton
        TimeUIText = RacingModeGameManager.instance.TimeUIText;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (TimeToStartRace >= 0.0f)
            {
                TimeToStartRace -= Time.deltaTime;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, TimeToStartRace);

            }
            else if (TimeToStartRace < 0.0f)
            {
                photonView.RPC("StartRace", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void SetTime(float time)
    {
        if (time > 0.0f)
        {
            TimeUIText.text = time.ToString("F1");
        }
        else
        {
            // The countdown is over
            TimeUIText.text = "";
        }
    }

    [PunRPC]
    public void StartRace()
    {
        GetComponent<CarMovement>().ControlsEnabled = true;
        this.enabled = false;
    }
}
