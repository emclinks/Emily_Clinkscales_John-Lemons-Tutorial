using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    //Image fade durations
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    //Objects referenced
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    //Conditions
    bool m_IsPlayerAtExit;
    float m_Timer;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsPlayerAtExit)
        {
            Endlevel();
        }
    }

    void Endlevel()
    {
        m_Timer = Time.deltaTime;
        
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            Application.Quit ();
        }
    }
}
