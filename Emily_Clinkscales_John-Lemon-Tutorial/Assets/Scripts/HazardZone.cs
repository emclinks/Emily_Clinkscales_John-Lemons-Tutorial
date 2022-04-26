using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour;
{
    //Connecting key objects and other code!
    public Transform player;
    GameEnding gameEnding;

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player)
        {
            gameEnding.PlayerDied();
        }
    }
}