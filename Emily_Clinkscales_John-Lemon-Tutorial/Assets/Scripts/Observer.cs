using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    PlayerMovement playerMovement;
    public AudioSource boom;
    public ParticleSystem bloosh;

    bool m_IsPlayerInRange;

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == player)
        {
            if(playerMovement.m_IsPlayerEctod == false)
            {
                m_IsPlayerInRange = true;
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if(m_IsPlayerInRange)
        {
            Vector3 direction = player.position = transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray, out raycastHit))
            {
                if(raycastHit.collider.transform == player)
                {
                    playerMovement.TakeDamage();
                }
            }
        }
    }

    public void GhostDeath()
    {
        boom.Play();
        bloosh.Play();
    }
}
