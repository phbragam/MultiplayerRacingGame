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
        // RacingModeGameManager.instance represents an instance created in scene, in this case TimeUIText from the Canvas in scene that is attached to RacingModeGameManager
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
