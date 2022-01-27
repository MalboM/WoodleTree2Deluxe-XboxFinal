using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAddBerries : MonoBehaviour
{
    [SerializeField] TPC player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPC>();

        if (player)
            Debug.LogError("PLAYER FOUND");
    }

    void Update()
    {
        if (XboxOneInput.GetKeyDown(XboxOneKeyCode.GamepadButtonY))
            AddBerries(500);
            
    }

    void AddBerries(int berriesToAdd)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<TPC>();

            if (player == null)
            {
                Debug.LogError("PLAYER NOT FOUND");
                return;
            }
            else
            {
                Debug.LogError("PLAYER FOUND");
            } 
        }

        //AGGIUNGI BACCHE ROSSE AL CONTEGGIO DEL GIOCATORE
        player.blueberryCount += berriesToAdd;
        player.berryCount += berriesToAdd;

        Debug.LogError("ADDED BERRIES");
    }
}
