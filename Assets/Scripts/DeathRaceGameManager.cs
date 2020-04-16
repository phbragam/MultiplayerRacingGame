using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathRaceGameManager : MonoBehaviour
{
    public GameObject[] PlayerPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int randomPosition = Random.Range(-15, 15);
                
                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, new Vector3(randomPosition, 0, randomPosition), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
