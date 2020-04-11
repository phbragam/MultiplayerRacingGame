using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] SelectablePlayers;
    public int PlayerSelectionNumber;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSelectionNumber = 0;

        ActivatePlayer(PlayerSelectionNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivatePlayer(int x)
    {
        foreach (GameObject SelectablePlayer in SelectablePlayers)
        {
            SelectablePlayer.SetActive(false);
        }
        SelectablePlayers[x].SetActive(true);
    }

    public void NextPlayer()
    {
        PlayerSelectionNumber += 1;
        if(PlayerSelectionNumber >= SelectablePlayers.Length)
        {
            PlayerSelectionNumber = 0;
        }

        ActivatePlayer(PlayerSelectionNumber);
    }

    public void PreviousPlayer()
    {
        PlayerSelectionNumber -= 1;

        if (PlayerSelectionNumber < 0)
        {
            PlayerSelectionNumber = SelectablePlayers.Length - 1;
        }
        ActivatePlayer(PlayerSelectionNumber);
    }
}
